# zulily Survey Test Summary

A live working demo of this project can be found here
http://menchions.com:7000

### Prerequisites

.NET Core on your local Development computer

Visual Studio 2017 on your local Development computer (Untested but may work on Visual Studio 2015)

Docker on your server

git on your server

## Deployment on Local Development Computer
* Clone this Repo to your local computer
* Load the .SLN solution file with Visual Studio 2017
* Debug > Start Debugging in VS2017 menu


## Deployment on Server

* Install Docker on you favorite operating system. We use Unbuntu Linux
* Install git on the same operating system
* Clone this repository
```
$ git clone https://github.com/smenchions/zulilyTest.git
```
* Ensure you have access to a MongoDB server. Currently we set the connection string to it from the appsettings.json file. Edit that setting.
* From the directory which contains the Dockerfile pulled from git run the following command
```
docker build -t zulilyTest .
```
* After your Docker image is built from the Dockerfile, run the following to start up the image
```
docker run --name zulilyTest -d -p 7000:5000 zulilyTest
```
* The above command will make the web site for this project accessible on port 7000 of your operating system. Ensure you have this port open on your local server.
* You will have to hit your newly created website on port 7000 a few times before it comes up in your browser. It take 10 to 30 seconds for .NET Core to start on a linux server.
* Feel free to change this to any free port, besides 7000.  The Docker image serves http traffic from port 5000, which is shown above and not configurable.


## Running the tests

Run the following command on your local development computer's command line which has .NET Core already installed on it. Do it from the \UnitTest directory of this project
```
dotnet test
```


## Built With

* [.Net Core](https://www.microsoft.com/net/core/platform)
* [Docker](https://www.docker.com/)
* [MongoDB](https://www.mongodb.com/)
* [ASP.Net MVC](https://www.asp.net/mvc)
* [ASP.Net Web API](https://www.asp.net/web-api)
* [JSON.Net](http://www.newtonsoft.com/json)
* [KnockoutJS](http://knockoutjs.com/)
* [bower](https://bower.io/)


## Authors

* **Shane Menchions** - *Initial work* - [LinkedIn](https://linkedin.com/in/shane)

## License

This project is licensed under the MIT License

