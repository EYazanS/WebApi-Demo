# WebApi-Demo

This a demo api.

You only need to clone the repo and run, it will auto generate the DB

once its runnign, Swagger UI  will open, 

go to the Users end point auth, and send "Guest" as both username and password, it will return a token, put the token inside the Authorize button in swagger.

Prefix it witth Bearer, ex: Bearer {token}

# What does it provide?


This template provides the following

Swagger UI, already initilized so you can instantly use it with security that can be disables easily.

base class for all of your db entities, that will auto add Id column, the id column can be of any type, but it defaults to Guid to the entire system.

Base Repository and manager that added the basic functions that can be used in any repository in the future.

Ecample controller for people that has all the CRUD operations.
