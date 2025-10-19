# Garmin Filter

A project for allowing extended searching/filtering of Garmin Apps based on e.g. Permissions.

## Running the project

### Backend

You can run the backend locally by using the `docker-compose.yml` found in the `/backend` folder. 

It'll build the project and compose up a container listening at port 8080.

You can then access the API's swagger interface on [localhost:8080](http://localhost:8080)

### Frontend

You can run the frontend locally by executing `flutter run -d chrome` within the `/frontend` folder.

As you can probably tell from the name, this requires flutter to be installed on your PC.

Also FYI the frontend is purely vibe-coded using Cursor (the backend is hand-built though)



## Hosting

The `hosting` folder (currently work-in-progress) will contain the necessary `docker-compose.yml` to compose up both backend and frontend from ghcr hosted images.

The backend will listen on port 8080, so if you want to expose it publicly you might want to put e.g. an nginx reverse proxy in front of it (advisably anyway really IMO)
