
char incoming;
byte CodoPosicion;
byte HombroArribaAbajoPosicion;
byte HombroAdelanteAtrasPosicion;

long tiempoEnSegundos = 0;
long UltimoSegundo = 0;

byte pos = 0;

#include <Servo.h>

Servo CodoServo;
Servo HombroArribaAbajoServo;
Servo HombroAdelanteAtrasServo;


void setup(){
  Serial.begin(9600);
  pinMode(13, OUTPUT);
  CodoServo.attach(9, 650, 2500);
  HombroArribaAbajoServo.attach(10);
  HombroAdelanteAtrasServo.attach(11);
  
}

void loop(){
  //tiempoEnSegundos=millis()/1000; //La funcion millis() devuelve el tiempo en milisegundos desde que el arduino comenzo a ejecutar el sketch
    if (Serial.available() > 3)
    {
      if(incoming = 'a')
      {
        if(pos == 0)
          pos = 255;
        else
          pos = 0;
        
         incoming = Serial.read();       
         CodoPosicion = Serial.read();
         HombroArribaAbajoPosicion = Serial.read();
         HombroAdelanteAtrasPosicion = Serial.read();
         Serial.write(incoming);
         Serial.print(CodoPosicion, DEC);
         Serial.print(HombroArribaAbajoPosicion, DEC);
         Serial.print(HombroAdelanteAtrasPosicion, DEC);

         Serial.print(pos, DEC);
         CodoServo.write(pos);
         HombroArribaAbajoServo.write(pos);
         HombroAdelanteAtrasServo.write(pos);
        
      }
   }

}

