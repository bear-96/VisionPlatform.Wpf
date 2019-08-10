﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using VisionPlatform.BaseType;
using VisionPlatform.Core;

namespace SenceDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (CameraFactory.CameraAssemblys.ContainsKey("VirtualCamera"))
            {
                CameraFactory.CameraAssemblyName = "VirtualCamera";
            }

        }

        private Scene scene;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scene?.Dispose();

                //if (File.Exists("查找5边型.json"))
                //{
                //    scene = Scene.Deserialize("查找5边型.json");
                //}
                //else
                //{
                //    string dllFile = @"..\..\..\..\VisionPlatform.VisionOpera\VisionPlatform.HalconOperaDemo\bin\Debug\VisionPlatform.HalconOperaDemo.dll";
                //    string serial = @"C:\Users\Public\Documents\MVTec\HALCON-17.12-Progress\examples\images";
                //    //string serial = @"00575388468";
                //    scene = new Scene("查找5边型", EVisionFrame.Halcon, dllFile, serial);
                //}

                scene = new Scene("查找5边型", EVisionFrame.VisionPro, @"E:\0. 临时目录\TestVpp.vpp");

                if (scene?.IsInit == true)
                {
                    RunningWindowGrid.Children.Clear();
                    RunningWindowGrid.Children.Add(scene.VisionFrame.RunningWindow);

                    ConfigWindowGrid.Children.Clear();
                    ConfigWindowGrid.Children.Add(scene.VisionFrame.ConfigWindow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "图像文件|*.bmp;*.png;*.jpg;*.jpgtif;*.tiff;*.giff;*.bmpf;*.jpgf;*.jpegf;*.jp2f;*.pngf;*.pcxf;*.pgmf;*.ppmf;*.pbmf;*.xwdf;*.ima|其他|*.*"
            };

            if (ofd.ShowDialog() == true)
            {
                string result = "";
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                scene?.ExecuteByFile(ofd.FileName, out result);
                stopwatch.Stop();

                RunningtimeTextBox.Text = scene.VisionFrame.VisionOpera.RunStatus.ProcessingTime.ToString("F3");
                Result1TextBox.Text = stopwatch.Elapsed.TotalMilliseconds.ToString();
                ResultConstantsTextBox.Text = result;
            }

        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string result = "";

                scene?.Execute(1000, out result);

                RunningtimeTextBox.Text = scene.VisionFrame.RunStatus.ProcessingTime.ToString("F3");
                ResultConstantsTextBox.Text = result;
                Result1TextBox.Text = scene.VisionFrame.RunStatus.TotalTime.ToString("F3");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            //SceneManager sceneManager = new SceneManager();
            //
            ////增
            //sceneManager.RegisterScene(scene);
            //
            ////删
            //sceneManager.DeleteScene("查找5边型");
            //
            ////改
            //sceneManager.Scenes["查找5边型"].OutputParamFile = "";
            //
            ////查
            //sceneManager.Scenes.ContainsKey("查找5边型");
            //
            ////遍历
            //foreach (var item in sceneManager.Scenes)
            //{
            //
            //}

            //显示
            //RunningWindowGrid.Children.Clear();
            //RunningWindowGrid.Children.Add(sceneManager.Scenes["查找5边型"].VisionFrame.RunningWindow);
            //
            ////执行
            //sceneManager.Scenes["查找5边型"].Execute(0, out var result);
            //
            ////配置界面
            


            if (scene != null)
            {
                Scene.Serialize(scene, "1.json");
            }
            scene?.Dispose();
            scene = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CameraFactory.RemoveAllCameras();
        }
    }
}
