# Project Title

Contact CMS


### Executing program

* How to run the program
```
dotnet run --project 'src/server/ContactSystem.Api/'
```

* How to run on docker
```
docker build --build-arg DOCKER_ENVIRONMENT=Production -t contact-cms . && docker run -e ASPNETCORE_ENVIRONMENT=Production -p 8080:8080 -p 5003:5003 contact-cms
```

Where `contact-cms` name of ur image on choice

P.S. Manually move to
<br> [http://localhost:5000/](http://localhost:5000/) (local)
<br> [https://localhost:5003/](https://localhost:5003/) (docker)

## Author

Yaroslav Fesiuk
[@LinkedIn](https://www.linkedin.com/in/yaroslav-fesiuk-2a28031a7/)
