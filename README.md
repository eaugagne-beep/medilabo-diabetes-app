#  Medilabo Diabetes App

##  Description

Medilabo Diabetes App est une application basée sur une architecture microservices permettant de :

- gérer les patients
- enregistrer des notes médicales
- évaluer le risque de diabète
- afficher les données via une interface web

Le projet est développé en .NET avec une architecture distribuée et conteneurisée via Docker.

---

##  Architecture

L’application est composée de plusieurs microservices :

- PatientService → gestion des patients (SQL Server)
- NoteService → gestion des notes médicales (MongoDB)
- AssessmentService → calcul du risque de diabète
- GatewayService (Ocelot) → point d’entrée unique
- PatientFront → interface utilisateur (MVC)

---

##  Fonctionnement

1. Le front appelle la Gateway
2. La Gateway redirige vers les microservices
3. AssessmentService appelle :
   - PatientService
   - NoteService
4. Les données sont stockées dans :
   - SQL Server (patients)
   - MongoDB (notes)

---

##  Technologies

- .NET 10
- ASP.NET Core Web API
- Ocelot (API Gateway)
- Entity Framework Core
- MongoDB
- SQL Server
- Docker & Docker Compose

---

##  Lancer le projet

### Prérequis
- Docker Desktop

### Commande

```bash
docker compose up --build
