using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LinqToWiki;
using LinqToWiki.Generated;
using LinqToWiki.Download;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;

namespace SearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchWiki_Click(object sender, RoutedEventArgs e)
        {
            SearchWiki(QuestionBox.Text); 
        }
        private void SearchWiki(string question)
        {
            Downloader.LogDownloading = true;
            var wiki = new Wiki("LinqToWiki.Samples", "https://en.wikipedia.org", "/w/api.php");
            var result = from s in wiki.Query.search(question)
                         select new { s.snippet };
            Write(result);
        }

        private void Write<TSource, TResult>(WikiQueryResult<TSource, TResult> results)
        {
            Write(results.ToEnumerable().Take(1));
        }

        private void Write<T>(IEnumerable<T> results)
        {
            var array = results.ToArray();

            foreach (var result in array)
            {              
                string answer = result.ToString();
                StringBuilder builder = new StringBuilder(answer);
                builder.Replace("{ snippet =", " ");
                builder.Replace("<span class=\"searchmatch\">", " ");
                builder.Replace("</span>", " ");
                builder.Replace("}", " ");
                string formattedAnswer = builder.ToString();
                // formattedAnswer = formattedAnswer.Substring(0, answer.LastIndexOf(".") + 1);
                AnswerBox.Text = formattedAnswer;
            }
                            
        }

        private void SearchGoogle_Click(object sender, RoutedEventArgs e)
        {
            const string apiKey = " AIzaSyAz85a3xdbGIWXA7mj5 - Rcw2fTqpAG5lsA";
            const string searchEngineId = "000724295970178438586:bv4bbjlttae";
            string query = QuestionBox.Text;
            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;
            IList<Result> paging = new List<Result>();
            paging = listRequest.Execute().Items;

            AnswerBox.Text = paging[0].Snippet;
        }
    }
}
