var l;

angular.module("app")
    .controller("ModsController", ["$scope", "Hub", "$timeout", function ($scope, Hub, $timeout) {

        $scope.state = "disconnected";
        $scope.loggedIn = false;
        $scope.mods = [];
        $scope.requestOptions = {
            Status: "Awaiting",
            Search: "",
            Limit: 20
        };
        $scope.loginFailed = false;
        $scope.loginData = {
            username: "",
            password: ""
        }

        l = $scope;

        // Declare the hub connection
        var hub = new Hub("ModsHub", {

            // Client side methods
            listeners: {
                // Handle incoming mods
                "sendMods": function (mods) {
                    $timeout(function () {
                        $scope.mods = mods;
                    });
                },
                "removeMod": function (id) {
                    $timeout(function () {
                        var index = -1;
                        for (var i = 0; i < $scope.mods.length; i++) {
                            if ($scope.mods[i].Id === id) {
                                index = i;
                                break;
                            }
                        }
                        if (index !== -1) {
                            $scope.mods = removeFromArray($scope.mods, index);
                        }
                    });
                },
                "setLoggedIn": function (state) {
                    $timeout(function () {
                        $scope.loggedIn = state;
                    });
                },
                "loginFailed": function () {
                    $timeout(function () {
                        $scope.loginFailed = true;
                        console.log("Ehh herb");
                    });
                }
            },

            methods: ["request", "accept", "deny", "loginUser"],

            errorHandler: function (error) {
                console.error(error);
            },

            stateChanged: function (state) {
                console.log(state);
                switch (state.newState) {
                    case $.signalR.connectionState.connecting:
                        $scope.state = "connecting";
                        break;
                    case $.signalR.connectionState.connected:
                        $scope.state = "connected";
                        break;
                    case $.signalR.connectionState.reconnecting:
                        $scope.state = "reconnecting";
                        break;
                    case $.signalR.connectionState.disconnected:
                        $scope.state = "disconnected";
                        break;
                }
                $scope.$apply();
            }
        });



        $scope.requestData = function () {
            if ($scope.state === "connected")
                hub.request($scope.requestOptions);
        }

        $scope.denyMod = function (mod) {
            if ($scope.state === "connected")
                hub.deny(mod);
        }

        $scope.acceptMod = function (mod) {
            if ($scope.state === "connected")
                hub.accept(mod);
        }

        $scope.$watch("requestOptions", function () {
            $scope.requestData();
        }, true);

        $scope.$watch("state", function () {
            console.log($scope.state);
            if ($scope.state === "connected")
                $scope.requestData();
        });

        $scope.login = function () {
            if ($scope.state === "connected") {
                console.log($scope.loginData);
                hub.loginUser($scope.loginData.username, $scope.loginData.password);
                $scope.loginData.password = "";
            }
        }

    }
    ]);