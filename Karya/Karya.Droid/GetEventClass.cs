using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Karya.Core;


namespace Karya.Droid
{
    public class GetEventClass
    {
        List<Event> levent = new List<Event>();
        public MobileServiceClient client = AuthClass.client;


        public async Task<List<Event>> readallevent()
        {
            try
            {
                var eventtable = client.GetSyncTable<Event>();
                levent = await eventtable.Select(x => x).ToListAsync();
                foreach (Event e in levent)
                {
                    if (e.Datetime < DateTime.Now)
                    {
                       // NotificationClass.removenot(e.Eventid);                       
                        bool delres=await DeleteEvent(e);
                        if(delres)
                            levent.Remove(e);
                    }
                        
                }
                return levent;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        public async void insertevent(Event ev)
        {
            try
            {
                var eventtable = client.GetSyncTable<Event>();
                await eventtable.InsertAsync(ev);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }



        //Delete all subjectlist or delete Subject table   

        public async Task DeleteAllEvents()
        {
            var localtable = client.GetSyncTable<Event>();
            try
            {
                List<Event> myCollection = await localtable.Select(x => x).ToListAsync();
                if (myCollection.Any())
                    foreach (Event ev in myCollection)
                    {
                        await localtable.DeleteAsync(ev);
                    }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }



        public async Task<bool> DeleteEvent(Event ev)
        {
            var localtable = client.GetSyncTable<Event>();
            try
            {        
                 await localtable.DeleteAsync(ev);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

        }


    }
}
