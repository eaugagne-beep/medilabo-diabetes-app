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


docker compose up --build

---

## 🌱 Green Code

### Objectif

Le Green Code vise à réduire l’impact environnemental du logiciel en limitant la consommation de ressources (CPU, mémoire, réseau) et en évitant les traitements inutiles.

---

### Enjeux identifiés dans le projet

- Multiplication des appels entre microservices (réseau)
- Absence de cache (recalculs inutiles)
- Chargement complet des données (notes)
- Logs non optimisés en production
- Images Docker non optimisées
- Absence de pagination

---

### Recommandations d’amélioration

- Mettre en place un cache pour éviter les appels répétés
- Réduire les appels entre microservices
- Implémenter la pagination pour limiter la mémoire
- Réduire la taille des réponses API
- Désactiver les logs inutiles en production
- Optimiser les images Docker (multi-stage build)
- Réutiliser les HttpClient pour éviter les allocations
- Éviter les traitements inutiles côté backend

---

### Point de vigilance

L’architecture microservices augmente naturellement les échanges réseau.  
Il est donc essentiel d’optimiser les appels entre services.
