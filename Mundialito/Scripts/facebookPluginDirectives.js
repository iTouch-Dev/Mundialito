﻿(function () {
    var createDirective, module, pluginName, _i, _len, _ref;

    module = angular.module('FacebookPluginDirectives', []);

    createDirective = function (name) {
        return module.directive(name, ['$location',function ($location) {
            return {
                restrict: 'C',
                link: function (scope, element, attributes) {
                    element.parent()[0].innerHTML = element.parent()[0].innerHTML.replace("#CURRENT_PAGE#", $location.absUrl().toString());
                    return typeof FB !== "undefined" && FB !== null ? FB.XFBML.parse(element.parent()[0]) : void 0;
                }
            };
        }]);
    };

    _ref = ['fbActivity', 'fbComments', 'fbFacepile', 'fbLike', 'fbLikeBox', 'fbLiveStream', 'fbLoginButton', 'fbName', 'fbProfilePic', 'fbRecommendations'];
    for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        pluginName = _ref[_i];
        createDirective(pluginName);
    }

}).call(this);