
char incoming;
byte CodoPosicion;
byte HombroArribaAbajoPosicion;
byte HombroAdelanteAtrasPosicion;

long tiempoEnSegundos = 0;
long UltimoSegundo = 0;

#include <Servo.h>

Servo CodoServo;
Servo HombroArribaAbajoServo;
Servo HombroAdelanteAtrasServo;


void setup(){
  Serial.begin(9600);
  pinMode(13, OUTPUT);
  CodoServo.attach(3);
  HombroArribaAbajoServo.attach(4);
  HombroAdelanteAtrasServo.attach(5);
  
}

void loop(){
  //tiempoEnSegundos=millis()/1000; //La funcion millis() devuelve el tiempo en milisegundos desde que el arduino comenzo a ejecutar el sketch
    if (Serial.available() > 3)
    {
      if(incoming = 'a')
      {
         incoming = Serial.read();       
         CodoPosicion = Serial.read();
         HombroArribaAbajoPosicion = Serial.read();
         HombroAdelanteAtrasPosicion = Serial.read();
         Serial.write(incoming);
         Serial.print(CodoPosicion, DEC);
         Serial.print(HombroArribaAbajoPosicion, DEC);
         Serial.print(HombroAdelanteAtrasPosicion, DEC);
         
         CodoServo.write(CodoPosicion);
         HombroArribaAbajoServo.write(HombroArribaAbajoPosicion);
         HombroAdelanteAtrasServo.write(HombroAdelanteAtrasPosicion);
        
      }
   }

}
