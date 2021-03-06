﻿using CSharpCrawler.Model;
using CSharpCrawler.Util;
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

namespace CSharpCrawler.Views
{
    /// <summary>
    /// InvokeWebAPI.xaml 的交互逻辑
    /// </summary>
    public partial class InvokeWebAPI : Page
    {
        public InvokeWebAPI()
        {
            InitializeComponent();

            InitCityList();
        }

        private void InitCityList()
        {
            this.combox_City.ItemsSource = Enum.GetNames(typeof(CityCode));
            this.combox_City.SelectedIndex = 0;
        }

        private async void btn_QueryWeather_Click(object sender, RoutedEventArgs e)
        {
            string city = this.combox_City.SelectedItem.ToString();
            string url = UrlUtil.WeatherQueryUrl.Replace("%s", ((int)Enum.Parse(typeof(CityCode), city)).ToString());
            string source =await WebUtil.GetHtmlSource(url,Encoding.UTF8);
            WeatherInfo weatherInfo = ResolveHtmlSource(source);

            ShowResult(weatherInfo);
            ShowWeather(weatherInfo);       
        }

        private WeatherInfo ResolveHtmlSource(string html)
        {
            return JsonUtil.ResolveWeather(html);
        }

        private void ShowResult(WeatherInfo weatherInfo)
        {
            Label label = new Label();
            label.FontSize = 15;
            label.Content = weatherInfo.ToString();
            this.groupbox_Result.Content = label;
        }

        private void ShowWeather(WeatherInfo weatherInfo)
        {

        }

        private async void btn_BingImage_Click(object sender, RoutedEventArgs e)
        {
            //Default 
            System.IO.Stream stream =await WebUtil.GetHtmlStreamAsync(UrlUtil.CNBingDailyImageUrl);         
            XmlUtil<BingDailyImages> xmlUtil = new XmlUtil<BingDailyImages>();
            BingDailyImages bingImages = xmlUtil.DeserializeXML(stream);
            this.lbl_Copyright.Content ="版权:" +  bingImages.Images.Copyright + "\r\n" + bingImages.Images.CopyrightLink;
            SetBackground(bingImages.Images.Url);       
        }

        private void SetBackground(string path)
        {
            path = UrlUtil.CNBingDailyImageBasicUrl  + path;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path);
            bi.EndInit();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = bi;
            Application.Current.MainWindow.Background = ib;
        }
    }
}
