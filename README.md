# TripPlanner (full-stack)
[![Build and Deploy](https://github.com/Nomi/TripPlanner-back-end/actions/workflows/TripPlannerAPI20221213230613.yml/badge.svg)](https://github.com/Nomi/TripPlanner-back-end/actions/workflows/TripPlannerAPI20221213230613.yml)

***Find the Docker Compose based version on the [DOCKERIZED-FINAL-fullstack-app](https://github.com/Nomi/TripPlanner-back-end/tree/DOCKERIZED-FINAL-fullstack-app) branch.***

## Table of Contents
1. [Introduction](#introduction)
 
2. [How to build and run server:](#how-to-build-and-run-server)
  - [Prerequisites](#prerequisites)
  - [Database creation and migration:](#database-creation-and-migration)
  - [Build](#build)
  - [Run](#run)
   
3. [How to access the application:](#how-to-access-the-appliation)
  - [API Spec](#api-spec)
  - [Front-end](#front-end)
  - [Front-end (Admin Panel)](#front-end-admin-panel)

## Introduction
The full-stack version of the TripPlanner API Web Application. A social network for people who love road trips.
Built using the following stack: C# (.NET 5), Entity Framework (the ORM), JavaSript, ReactJS, and MS SQL Server, among others.

## How to build and run server
\[frontend non-static/source codes' instructions are in the readme.md in its folder,but as long as no changes are made to the frontend sourcecode, you can just ignore building that because wwwroot contains a build (which is the latest as of writing this).\] 

([Here's the direct link to the subdirectory containing the frontend sourcode, in case needed.](https://github.com/Nomi/TripPlanner-back-end/tree/FINAL-fullstack-app/TripPlannerAPI/wwwroot/.ReactSourceCode/TripPlanner-front-end))

### Prerequisites
Before you begin, ensure you have the following prerequisites installed:
- .NET SDK: You can download and install it from [here](https://dotnet.microsoft.com/download).
- MS SQL Server (**change connection string as needed**).
- dotnet Entity Framework Core tools.


### Database creation and migration
You will need to do this process on the first run, **BUT, also** after any changes to the Model.
```bash
dotnet ef migrations add *REPLACE_WITH_NAME_FOR_MIGRATION*
```
```bash
dotnet ef database update
```
### Build
```bash
dotnet build
```
### Run
```bash
dotnet run
```

## How to access the appliation

***If you have a different URL for the server, replace the base part of the URLs in the following before using them.***

### API Spec: 

URL:

http://127.0.0.1:7034/api

or the full url: *http://127.0.0.1:7034/api/index.html*

### Front-end:

http://127.0.0.1:7034/


### Front-end (Admin Panel):

To make sure Admin account exists, send an empty HTTP Post request to the endpoint "​/api​/Account​/ensure-Admin@101-created" (if it needs content type, try the JSON content type).
I realize now after so long that there were better ways to handle default Admin account creation (and honestly, a lot of things) but I have improved on it and everything else in the WebAPIs I made afterwards.


http://127.0.0.1:7034/admin


Default Admin Username: Admin@101


Default Admin Password: Admin@101
