﻿using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

//Estas referencias son necesarias para usar GLIDE
using GHI.Glide;
using GHI.Glide.Display;
using GHI.Glide.UI;

namespace Practica3DSCC
{
    public partial class Program
    {

        //Objetos de interface gráfica GLIDE
        private GHI.Glide.Display.Window controlWindow;
        private GHI.Glide.Display.Window camaraWindow;
        private Button btn_start;
        private Button btn_stop;
        private SensorProximidad sensor;
        enum Estado { SENSORON, SENSOROFF, MONITOREO };
        Estado estado;
        private TextBlock text;

   

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            /*******************************************************************************************
            Modules added in the Program.gadgeteer designer view are used by typing 
            their name followed by a period, e.g.  button.  or  camera.
            
            Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
                button.ButtonPressed +=<tab><tab>
            
            If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
                GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
                timer.Tick +=<tab><tab>
                timer.Start();
            *******************************************************************************************/


            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");

            //Carga las ventanas
            controlWindow = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.controlWindow));
            camaraWindow = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.camaraWindow));
            GlideTouch.Initialize();
            sensor = new SensorProximidad(extender);
  
           
            //Inicializa los botones en la interface
        
            camera.BitmapStreamed += camera_BitmapStreamed;
            text  = (TextBlock)controlWindow.GetChildByName("status");
            btn_start = (Button)controlWindow.GetChildByName("start");
            btn_stop = (Button)controlWindow.GetChildByName("stop");
            sensor.ObjectOn += sensor_ObjectOn;
            sensor.ObjectOff += sensor_ObjectOff;
            btn_start.TapEvent += btn_start_TapEvent;
            btn_stop.TapEvent += btn_stop_TapEvent;
           
            //Selecciona mainWindow como la ventana de inicio
            Glide.MainWindow = controlWindow;
        }

        private void cambiarEstado(Estado es) {
            switch (es)
            {
                case Estado.SENSOROFF:
                    Debug.Print("cambiar a off");
                    text.Text = "Monitoreo OFF";
                    Glide.MainWindow = controlWindow;
                    break;

                case Estado.SENSORON:
                    text.Text="Monitoreo ON";
                    camera.StopStreaming();
                    Glide.MainWindow = controlWindow;
                    break;
            
                case Estado.MONITOREO:
                    Glide.MainWindow = camaraWindow;
                    camera.StartStreaming();
                    break;
                default:
                    break;

            }                }

      
        private void camera_BitmapStreamed(Camera sender, Bitmap e)
        {
            if(estado.Equals(Estado.MONITOREO)){
            displayT35.SimpleGraphics.DisplayImage(e, 0, 0);
            }
           // camera.StopStreaming();
        }

        void sensor_ObjectOff()
        {
            Debug.Print("Evento objectOFF lanzado");
            estado = Estado.SENSORON;
            cambiarEstado(estado);
            
        }

        void sensor_ObjectOn()
        {
            estado = Estado.MONITOREO;
            cambiarEstado(estado);
            Debug.Print("Evento objectOn lanzado");
        }

        void btn_stop_TapEvent(object sender)
        {
            estado = Estado.SENSOROFF;
            cambiarEstado(estado);
            sensor.StopSampling();
            Debug.Print("Stop");
        }

        void btn_start_TapEvent(object sender)
        {
            estado=Estado.SENSORON;
            cambiarEstado(estado);
            sensor.StartSampling();
            Debug.Print("Start");
        }
    }
}
