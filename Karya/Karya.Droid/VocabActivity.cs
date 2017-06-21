using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Karya.Core;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Karya.Droid
{
    [Activity(Label = "VocabActivity")]
    public class VocabActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.Vocablayout);

            GetVocabClass getvocab = new GetVocabClass();
            Vocab v = await GetVocabClass.GetVocab();
            TextView word = FindViewById<TextView>(Resource.Id.magicwordtext);
            word.Text = v.Word;
            TextView meaning = FindViewById<TextView>(Resource.Id.meaningtext);
            meaning.Text = v.Meaning;
            TextView usage = FindViewById<TextView>(Resource.Id.usagetext);
            usage.Text = v.Usage;
            TextView antonym = FindViewById<TextView>(Resource.Id.antonymtext);
            antonym.Text = v.Antonym;
            TextView category = FindViewById<TextView>(Resource.Id.categorytext);
            category.Text = v.Category;
            TextView synonym = FindViewById<TextView>(Resource.Id.synonymtext);
            synonym.Text = v.Synonym;
            

        }
    }
}