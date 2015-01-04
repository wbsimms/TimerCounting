using System;
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
using Gadgeteer.SocketInterfaces;

namespace TimerCounting
{
    public partial class Program
    {
        private GT.SocketInterfaces.DigitalOutput nine, eight, seven, six, five, four, three;
        private ArrayList pins = new ArrayList();
        private GT.SocketInterfaces.InterruptInput interruptInput;
        void ProgramStarted()
        {
            Debug.Print("Program Started");
            interruptInput = extender.CreateInterruptInput(GT.Socket.Pin.Nine, GlitchFilterMode.Off, ResistorMode.PullUp, InterruptMode.RisingEdge);
            button.ButtonPressed += button_ButtonPressed;
            nine = breadBoardX1.CreateDigitalOutput(GT.Socket.Pin.Nine, false);
            eight = breadBoardX1.CreateDigitalOutput(GT.Socket.Pin.Eight, false);
            seven = breadBoardX1.CreateDigitalOutput(GT.Socket.Pin.Seven, false);
            six = breadBoardX1.CreateDigitalOutput(GT.Socket.Pin.Six, false);
            five = breadBoardX1.CreateDigitalOutput(GT.Socket.Pin.Five, false);
            four = breadBoardX1.CreateDigitalOutput(GT.Socket.Pin.Four, false);
            three = breadBoardX1.CreateDigitalOutput(GT.Socket.Pin.Three, false);
            pins.Add(three);
            pins.Add(four);
            pins.Add(five);
            pins.Add(six);
            pins.Add(seven);
            pins.Add(eight);
            pins.Add(nine);
        }

        private int currentNumber = 0;
        void interruptInput_Interrupt(InterruptInput sender, bool value)
        {
            bool v = interruptInput.Read();
            if (v)
                SetNextNumber();
            if (currentNumber >= 9)
                currentNumber = 0;
        }


        private bool isOn = false;
        void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            if (!isOn)
            {
                interruptInput.Interrupt += interruptInput_Interrupt;
                isOn = true;
            }
            else
            {
                interruptInput.Interrupt -= interruptInput_Interrupt;
                isOn = false;
            }
        }

        public void SetNextNumber()
        {
            ClearPins();
            foreach (int i in GetNumberPins(currentNumber++))
            {
                ((DigitalOutput)pins[i - 3]).Write(true);
            }
        }


        public void ClearPins()
        {
            foreach (DigitalOutput o in pins)
            {
                o.Write(false);
            }
        }

        public int[] GetNumberPins(int number)
        {
            switch (number)
            {
                case 0:
                    return new int[] {3,5,6,7,8,9 };
                case 1:
                    return new int[] { 7,8 };
                case 2:
                    return new int[] { 9,8,4,5,6};
                case 3:
                    return new int[] { 9,8,4,7,6 };
                case 4:
                    return new int[] { 3,4,8,7 };
                case 5:
                    return new int[] { 9,3,4,7,6};
                case 6:
                    return new int[] { 3,4,5,7,6, };
                case 7:
                    return new int[] { 9,8,7 };
                case 8:
                    return new int[] { 3,4,5,6,7,8,9 };
                case 9:
                    return new int[] { 3,4,9,8,7 };
                default:
                    return new int[0];
            }
        }
    }
}
