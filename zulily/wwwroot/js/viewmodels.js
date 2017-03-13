
/**
 * PLEASE NOTE:  This JS code is not under test.  Due to time constraints I was unable to create any Unit Tests for it.
    Unit tests for the View Models and the back and forth from the server data would be a must if more time was available.
 */


var ZulilyHPViewModel = function () {
    var self = this;
    //A simple flag to indicate the there is data coming from the API
    self.loading = ko.observable(false);
    // if a message is here, it will show up at the top of the page with a red alert. Typically HTTP errors will get here
    // from the fail methods
    self.error = ko.observable();
    //the user who gives us their email address and fills out the survey
    self.user = new User();
    // for each age and gender there is a survey template of images appropriate to those selections
    self.surveyTemplate = new Survey();
    // the users survey selections.  Their own survey data.
    self.surveyResult = new Survey();
    // how many images the user has highlighted
    self.selectedImageCount = ko.observable(0);

    self.loading.subscribe(function (newValue) {
        if (newValue) {
            self.error(null);
        }
    });

    //once the user is loaded from the back end, do things
    self.user.sId.subscribe(function (newValue) {
        if (newValue) {
            $('#captureEmail').modal('hide');
            self.surveyResult.sUserId(newValue);
            self.loadExistingUserSurvey();
        } else {
            $('#captureEmail').modal('show');
        }
    });

    // all view models once loaded on the page get their load method called
    self.load = function () {   
        self.getUser();
    };


    // get the user for this survey. Look to see if they have a cookie on their browser which indicates they are in progress
    // if no cookie, see if we have their email address. Either credential could get us a User from the server
    self.getUser = function () {
        var url = '/api/user/';
        var UserId = $.cookie('User');
        if (!UserId && !self.user.email()) {
            $('#captureEmail').modal('show');
            return;
        }           
        if (self.user.email())
            url = url + 'email/' + self.user.email();
        else
            url = url + UserId;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: 'application/json; charset=utf-8'
        }).success(function (results) {
            self.loading(false);
            if (results) {
                self.user.fromServerModel(results);
            } else {
                self.saveUser();
            }
        }).fail(function (err) {
            self.loading(false);
            self.error(err.responseText);
        });
    };


    // save the user to the server
    self.saveUser = function () {
        if (self.user.emailValid() == false)
            return;
        $.ajax({
            url: '/api/user/',
            type: 'PUT',
            data: JSON.stringify(self.user.toServerModel()),
            dataType: 'json',
            processData: false,
            contentType: 'application/json; charset=utf-8'
        }).success(function (results) {
            if (results) {
                self.user.fromServerModel(results);
            }
        }).fail(function (err) {
            self.loading(false);
            self.error(err.responseText);
        });
    };

    // load any existing surveys that the user has on our server. They could be complete or in progress
    self.loadExistingUserSurvey = function () {
        $.ajax({
            url: '/api/survey/user/' + self.user.sId(),
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: 'application/json; charset=utf-8'
        }).success(function (results) {
            self.loading(false);
            if (results) {
                self.surveyResult.fromServerModel(results);
                self.surveyResult.type('UserResult'); //should be an JS enum equiv
                self.loadSurveyTemplate();
            }
        }).fail(function (err) {
            self.loading(false);
            self.error(err.responseText);
        });
    };

    // for the given age and gender selections from the user, load the survey template to get the correct images
    self.loadSurveyTemplate = function () {
        self.surveyTemplate.fromServerModel({});
        $.ajax({
            url: '/api/survey/',
            type: 'POST',
            data: JSON.stringify(self.surveyResult.toServerModel()),
            dataType: 'json',
            processData: false,
            contentType: 'application/json; charset=utf-8'
        }).success(function (results) {
            self.loading(false);
            self.surveyTemplate.fromServerModel(results[0]); //assumes only one survey
            $.each(self.surveyResult.images(), function (i, img) {
                $.each(self.surveyTemplate.images(), function (i, templateImg) {
                    if (img.sId() == templateImg.sId()) {
                        templateImg.selected(true);
                        self.selectedImageCount(self.selectedImageCount() + 1);
                    }
                });
            });
            if (self.selectedImageCount() > 0) {
                $('#success').modal('show');
            }
        }).fail(function (err) {
            self.loading(false);
            self.error(err.responseText);
        });
    };

    // save the users survey.  The survey does not need to be fully completed, but could be in progress when saved here
    self.saveSurvey = function () {
        $.ajax({
            url: '/api/survey/',
            type: 'PUT',
            data: JSON.stringify(self.surveyResult.toServerModel()),
            dataType: 'json',
            processData: false,
            contentType: 'application/json; charset=utf-8'
        }).success(function (results) {
            self.loading(false);
            self.surveyResult.fromServerModel(results);
            self.loadSurveyTemplate();
        }).fail(function (err) {
            self.loading(false);
            self.error(err.responseText);
        });
    };

    //click handler for the images
    self.selectImage = function (img) {
        img.selected(!img.selected());
        if (img.selected())
            self.selectedImageCount(self.selectedImageCount() + 1);
        else
            self.selectedImageCount(self.selectedImageCount() - 1);

    };

    //final save of the survey now that the images have been selected
    self.submitStyle = function () {
        self.surveyResult.images.removeAll();
        $.each(self.surveyTemplate.images(), function (i, img) {
            if (img.selected()) {
                self.surveyResult.images.push(img);
            }
        });
        self.saveSurvey();
    };

    // earse the cookie, erase the survey on the server and reset VM vars
    self.logout = function () {
        $.removeCookie('User');
        //reset the vars
        self.user.fromServerModel({});       
        self.surveyTemplate.fromServerModel({});
        self.selectedImageCount(0);
        $('#captureEmail').modal('show');
        $.ajax({
            url: '/api/survey/',
            type: 'DELETE',
            data: JSON.stringify(self.surveyResult.toServerModel()),
            dataType: 'json',
            processData: false,
            contentType: 'application/json; charset=utf-8'
        }).success(function (results) {
            self.loading(false);
            self.surveyResult.fromServerModel({});
        }).fail(function (err) {
            self.loading(false);
            self.error(err.responseText);
        });
    };


};
