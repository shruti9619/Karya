﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Karya.Core;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.IO;

namespace Karya.WinPhone
{
    public class GetSubjectClass
    {

        public static MobileServiceClient client = AuthClass.client;


        // Retrieve the all subjects from the database.   
        public async Task<List<Subject>> ReadAllSubject()
        {

            List<Subject> SubjectList;
            try
            {
                var localtable = client.GetSyncTable<Subject>();

                List<Subject> myCollection = await localtable.Select(x => x).ToListAsync();
                SubjectList = new List<Subject>(myCollection);
                return SubjectList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }



        //Update existing subject  
        public async Task<bool> UpdateSubject(Subject newsub, string oldsubname)
        {
            var localtable = client.GetSyncTable<Subject>();
            Subject existingsubject = null;
            System.Diagnostics.Debug.WriteLine("1");
            StorageFolder oldfolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync(oldsubname);
            List<Subject> eslist = await localtable.Where(u => u.Subjectname == oldsubname).Select(u => u).ToListAsync();
            if (eslist.Any())
                existingsubject = eslist.FirstOrDefault();

            if (existingsubject != null)
            {
                existingsubject.Subjectname = newsub.Subjectname;
                existingsubject.Teacherid = newsub.Teacherid;
                existingsubject.Teachername = newsub.Teachername;
                await localtable.UpdateAsync(existingsubject);
                if (newsub.Subjectname != oldsubname)
                    await oldfolder.RenameAsync(existingsubject.Subjectname);
                MessageDialog m = new MessageDialog("Subject Updated");
                await m.ShowAsync();

                try
                {
                    App.addtodebug("push async to come");
                    await client.SyncContext.PushAsync();
                }
                catch (Exception e)
                {
                    App.addtodebug("push prob " + e.Message);
                    return true;
                }

                return true;

            }
            else
                return false;

        }



        // Insert the new subject in the Subject table.   
        public async Task<bool> Insert(string teachername, string subname)
        {

            var localtable = client.GetSyncTable<Subject>();
            Subject existingsubject = null;
            System.Diagnostics.Debug.WriteLine("1");
            IMobileServiceTableQuery<Subject> s = localtable.Where(u => u.Subjectname == subname).Select(u => u);
            List<Subject> slist = await s.ToListAsync();
            if (slist.Any())
                existingsubject = slist.FirstOrDefault();
            // existingsubject = localtable.Where(u => u.Subjectname == subname).Select(u => u) as Subject;

            System.Diagnostics.Debug.WriteLine("2");

            if (existingsubject == null)
            {

                Subject newsub = new Subject();
                newsub.Subjectname = subname;
                newsub.Teachername = teachername;
                newsub.createdAt = DateTime.Today;
                newsub.Userid = AuthClass.commonuser.Userid;
                int teacherid = 0;
                System.Diagnostics.Debug.WriteLine("3");
                // we have to actually look up the teachername from the cloud, if its not there we create our own here using max. save to phone


                int sid;

                newsub.Teacherid = teacherid + 1;

                System.Diagnostics.Debug.WriteLine("4");
                //the next few steps are to get the total no of subjects in the list and assign maximum+1 value to the next sub id
                // through this is is surely possible for two users on the cloud to have the same subjectid for any subject but
                // it is not possible to have a combination of the same userid and subjectid
                //hence there will be no database errors

                List<int> checklistforempty = await localtable.Select(x => x.Subjectid).ToListAsync();
                System.Diagnostics.Debug.WriteLine("5");

                bool isfilled = checklistforempty.Any();
                System.Diagnostics.Debug.WriteLine("6");

                if (!isfilled)
                    sid = 0;
                else
                    sid = checklistforempty.Count();


                newsub.Subjectid = sid + 1;
                newsub.id = newsub.Subjectid.ToString();
                //int insertcheck = 0;
                System.Diagnostics.Debug.WriteLine("7");

                await localtable.InsertAsync(newsub);

                //var remotetable = client.GetTable<Subject>();
                //try
                //{
                //    App.addtodebug("push async to come");
                //    await client.SyncContext.PushAsync();
                //}
                //catch (Exception e)
                //{
                //    App.addtodebug("push prob "+e.Message);
                //}
                System.Diagnostics.Debug.WriteLine("8");
                // Get the app's local folder.
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;

                // Create a new subfolder in the current folder.
                // Raise an exception if the folder already exists.
                string Foldername = newsub.Subjectname;
                StorageFolder newFolder =
                await localFolder.CreateFolderAsync(Foldername, CreationCollisionOption.FailIfExists);
                System.Diagnostics.Debug.WriteLine("9");

                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(client.SyncContext.PendingOperations);
                return false;
            }
        }




        //Delete specific subject 
        public async Task<bool> DeleteSubject(Subject sub)
        {
            var localtable = client.GetSyncTable<Subject>();
            Subject existingsubject = null;
            System.Diagnostics.Debug.WriteLine("1");
            string Foldername = sub.Subjectname;
            StorageFolder subFolder;


            subFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync(Foldername);
            List<Subject> eslist = await localtable.Where(u => u.Subjectid == sub.Subjectid).Select(u => u).ToListAsync();
            if (eslist.Any())
                existingsubject = eslist.FirstOrDefault();

            if (existingsubject != null)
            {
                await localtable.DeleteAsync(existingsubject);
                await subFolder.DeleteAsync();

                System.Diagnostics.Debug.WriteLine("folder deleted");
                MessageDialog m = new MessageDialog("Subject Deleted");
                await m.ShowAsync();

                try
                {
                    App.addtodebug("push async to come");
                    await client.SyncContext.PushAsync();
                }
                catch (Exception e)
                {
                    App.addtodebug("push prob " + e.Message);
                    return true;
                }
                return true;

            }
            else
                return false;

        }




        //Delete all subjectlist or delete Subject table   

        public async Task DeleteAllSubjects()
        {


            var localtable = client.GetSyncTable<Subject>();


            try
            {
                List<Subject> myCollection = await localtable.Select(x => x).ToListAsync();
                if (myCollection.Any())
                    foreach (Subject sub in myCollection)
                    {
                        string Foldername = sub.Subjectname;
                        StorageFolder subFolder
                        = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync(Foldername);
                        await subFolder.DeleteAsync();
                        await localtable.DeleteAsync(sub);
                    }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}