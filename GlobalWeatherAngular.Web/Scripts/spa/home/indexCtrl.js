(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function indexCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.loadingCountries = true;
        $scope.loadingWeather = true;

        $scope.country = '';
        $scope.countries = [];
        $scope.city = '';
        $scope.cities = [];
        $scope.weather = {};
        $scope.loadData = loadData;

        $scope.getCityData = function (country) {
            if (country != null) {
                apiService.get("/api/city/list?countryName=" + country, null, citiesLoadCompleted, citiesLoadFailed);
            } else {
                clearCities();
            }
        };

        $scope.getWeatherData = function (city) {
            if (city != null) {
                apiService.get("/api/weather/get?cityName=" + city, null, weatherLoadCompleted, weatherLoadFailed);
            } else {
                clearWeather();
            }
        };

        function loadData() {
            apiService.get("/api/country/list", null, countriesLoadCompleted, countriesLoadFailed);
        }

        function countriesLoadCompleted(result) {
            $scope.countries = result.data;
            $scope.loadingCountries = false;
        }

        function countriesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function citiesLoadCompleted(result) {
            if (result.data.length > 0) {
                $scope.cities = result.data;
                $scope.city = $scope.cities[0];
                clearWeather();
                $scope.getWeatherData($scope.city);
            } else {
                notificationService.displayWarning("No data found.");
                clearCities();
            }
        }

        function citiesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function weatherLoadCompleted(result) {
            if (result.data.Location != null) {
                $scope.weather = result.data
            } else {
                notificationService.displayWarning("No data found.");
                clearWeather();
            }
            $scope.loadingWeather = false;
        }

        function weatherLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearCities() {
            $scope.city = '';
            $scope.cities = [];
            clearWeather();
        }

        function clearWeather() {
            $scope.weather = {};
        }
        
        loadData();
    }
})(angular.module('globalWeather'));
