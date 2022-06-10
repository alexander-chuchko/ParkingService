﻿// See https://aka.ms/new-console-template for more information




using CoolParking.BL;
using CoolParking.BL.Models;
using CoolParking.BL.Services;





ParkingService parkingService = new ParkingService(new TimerService(), new TimerService(), new LogService(Settings.logFilePath));
UserInterface navigation = new UserInterface(parkingService);
navigation.StartApplication();








