using System;
using Microsoft.SPOT;
using GTM = Gadgeteer.Modules;
using GT = Gadgeteer;

namespace Practica3DSCC
{
    // Referencia tipo "delegate" para función callback ObjectOn
    public delegate void ObjectOnEventHandler();

    // Referencia tipo "delegate" para función callback ObjectOff
    public delegate void ObjectOffEventHandler();
      

    /*
     * Clase SensorProximidad, encapsula el funcionanmiento del sensor de proximidad infrarrojo.
     * Esta clase gestiona los dos componentes del sensor: el LED infrarrojo y el foto-transistor.
     * Además, dispara dos eventos: ObjectOn y ObjectOff cuando el sensor detecta la presencia o
     * ausencia de un objeto.
     */
    class SensorProximidad
    {
        //EVENTO ObjectOff: Disparar este evento cuando el sensor detecte la ausencia del objeto
        public event ObjectOffEventHandler ObjectOff;

        //EVENTO ObjectOn: Disparar este evento cuando el sensor detecte la presencia de un objeto
        public event ObjectOnEventHandler ObjectOn;

        GT.SocketInterfaces.DigitalOutput salida;
        GT.SocketInterfaces.AnalogInput entrada;
        Double valor=0.0;
        Double umbral = 3.10;
        Boolean objetoDetectado = false;
       
         GT.Timer timer = new GT.Timer(250); // every second (1000ms)
        

     
        public SensorProximidad(GTM.GHIElectronics.Extender extender)
        {

            salida = extender.CreateDigitalOutput(GT.Socket.Pin.Four,false);
            entrada = extender.CreateAnalogInput(GT.Socket.Pin.Three);
            timer.Tick += timer_Tick;
            
            //TODO: Inicializar el sensor
        }

        void timer_Tick(GT.Timer timer)
        {
           valor= entrada.ReadVoltage();
           if (valor >= umbral)
           {
               Debug.Print("no hay nada");
               if (objetoDetectado) {
                   objetoDetectado = false;
                    ObjectOff();
               }
           
         
           }
           else {
               Debug.Print("hay un objeto a la vista");
               if (!objetoDetectado) {
                   objetoDetectado = true;
                   ObjectOn(); 
               }
           }
     
          
        }

        public void StartSampling()
        {
            //TODO: Activar el LED infrarrojo y empezar a muestrear el foto-transistor
            salida.Write(true);
            timer.Start();
            Debug.Print("led encendido");
        }

        public void StopSampling()
        {
            salida.Write(false);
            timer.Stop();
            Debug.Print("led apagado");
            //TODO: Desactivar el LED infrarrojo y detener el muestreo del foto-transistor
        }
    }
}
