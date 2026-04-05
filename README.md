#  Medilabo Diabetes App

##  Description

Application de gestion et d’évaluation du risque de diabète basée sur une architecture microservices.

Le projet permet :

* la gestion des patients
* la gestion des notes médicales
* le calcul du niveau de risque de diabète
* l’exposition des services via une API Gateway
* une interface utilisateur web (frontend MVC)

---

##  Architecture

Le projet est composé de plusieurs microservices :

* **PatientService** → gestion des patients (SQL Server)
* **NoteService** → gestion des notes (MongoDB)
* **AssessmentService** → calcul du niveau de risque
* **GatewayService (Ocelot)** → point d’entrée unique
* **PatientFront** → interface utilisateur

---

##  Sécurité

Les microservices sont sécurisés avec une authentification **Basic Auth**.

Identifiants par défaut :

* **username** : admin
* **password** : admin123

---

##  Docker

L’ensemble de l’application est conteneurisé avec Docker.

### Lancer l’application


docker compose up --build


---

##  Accès aux services

* Frontend : http://localhost:5125
* Gateway : http://localhost:5110
* Patient API : http://localhost:5261/swagger
* Note API : http://localhost:5148/swagger
* Assessment API : http://localhost:5027/swagger

---

##  Fonctionnement

1. Le frontend appelle la Gateway
2. La Gateway redirige vers les microservices
3. AssessmentService appelle :

   * PatientService
   * NoteService
4. Le niveau de risque est calculé et affiché

---


##  Green Code

###  Objectif

Réduire l’impact environnemental de l’application en optimisant les ressources.

###  Enjeux identifiés

* consommation CPU et mémoire
* appels réseau inutiles
* duplication de code
* conteneurs multiples

###  Recommandations

* limiter les appels API redondants
* mutualiser le code commun (authentification)
* optimiser les requêtes (SQL/MongoDB)
* éviter les objets inutiles en mémoire
* utiliser des images Docker légères
* centraliser la configuration

---



##  Améliorations possibles

* authentification plus robuste (JWT)
* sécurisation du frontend (login utilisateur)
* gestion des rôles

---

##  Technologies utilisées

* .NET 10
* ASP.NET Core
* Entity Framework Core
* MongoDB
* Ocelot (API Gateway)
* Docker & Docker Compose
* Swagger / OpenAPI

---

