/**
 * PLEASE NOTE:  This JS code is not under test.  Due to time constraints I was unable to create any Unit Tests for it.
    Unit tests for the View Models and the back and forth from the server data would be a must if more time was available.
 */







var Survey = function () {
    var self = this;
    self.sId = ko.observable();
    self.sUserId = ko.observable();
    self.gender = ko.observable();
    self.type = ko.observable();
    self.ageRange = ko.observable();
    self.images = ko.observableArray();
    self.fromServerModel = function (d) {
        if (!d)
            return;
        self.sId(d.sId);
        self.sUserId(d.sUserId);
        self.gender(d.gender);
        self.type(d.type);
        self.ageRange(d.ageRange);
        if (d.images) {
            d.images.map(function (img) {
                var i = new Image();
                i.fromServerModel(img);
                self.images.push(i);
            });
        } else {
            self.images.removeAll();
        }
    };

    self.toServerModel = function () {      
    return ko.toJS(self);
    };
};


var User = function () {
    var self = this;
    self.sId = ko.observable();
    self.firstName = ko.observable();
    self.lastName = ko.observable();
    self.email = ko.observable();
    self.emailValid = ko.observable();
    self.email.subscribe(function (newValue) {
        if (newValue == null || newValue.match(/^[^@]+@[^@]+\.[^@]+$/))
            self.emailValid(true);
        else
            self.emailValid(false);
    });

    self.fromServerModel = function (d) {
        if (!d)
            return;
        self.sId(d.sId);
        self.firstName(d.firstName);
        self.lastName(d.lastName);
        self.email(d.email);
    };

    self.toServerModel = function () {
        return ko.toJS(self);
    };
};


var Image = function () {
    var self = this;
    self.sId = ko.observable();
    self.sUserId = ko.observable();
    self.url = ko.observable();
    self.selected = ko.observable(false);
    self.tags = ko.observableArray();
    self.fromServerModel = function (d) {
        if (!d)
            return;
        self.sId(d.sId);
        self.url(d.url);
        if (d.tags) {
            d.tags.map(function (tag) { self.tags.push(tag) });
        } else {
            self.tags.removeAll();
        }
    };

    self.toServerModel = function () {
        return ko.toJS(self);
    };
};
