using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Karya.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace Karya.WinPhone
{
    public class GetVocabClass
    {


        static MobileServiceClient client;
        private static IMobileServiceTable<Vocab> vocabTable;

        public GetVocabClass()
        {
            client = AuthClass.client;
            vocabTable = client.GetTable<Vocab>();
        }


        public static async Task<Vocab> GetVocab()
        {

            Vocab fetchvocab = null;
            int randomid = 0;
            Random rand = new Random();

            IMobileServiceTableQuery<Vocab> query = vocabTable.OrderByDescending(v => v.Vocabid);
            List<Vocab> maxvocabidlist = await query.ToListAsync();
            if (maxvocabidlist.Any())
            {
                Vocab maxvocab = maxvocabidlist[0];

                //assign random number from one to max of vocab id
                randomid = rand.Next(1, maxvocab.Vocabid + 1);
               
            }

            IMobileServiceTableQuery<Vocab> vq = vocabTable.Select(x => x).Where(x => x.Vocabid == randomid);
            List<Vocab> vlist = await vq.ToListAsync();
            if (vlist.Any())
                fetchvocab = vlist.FirstOrDefault();

            return fetchvocab;
        }
    }
}
