/// <reference path="scripts.api.js" />
/// <reference path="lib/jquery.query.js" />

module("Api");

test("given_no_arguments_provided buildQuery_takes_values_from_query_string", function () {
    window.location.hash = "?from=2012-01-01&limit=10";

    var query = Api.buildQuery();

    equals(query.get("type"), "");
    equals(query.get("from"), "2012-01-01");
    equals(query.get("to"), "");
    equals(query.get("limit"), 10);
});

test("given_arguments_provided_buildQuery_overrides_values_from_query_string", function () {
    window.location.hash = "?type=rss&to=2012-01-01";

    var query = Api.buildQuery(/*type*/ "twitter");

    equals(query.get("type"), "twitter");
    equals(query.get("to"), "2012-01-01");
});

test("given_no_base_url_provided_getUrl_uses_default_one", function () {
    window.location.hash = "?type=rss";

    var url = Api.getUrl(/*baseUrl*/null);

    ok(url.indexOf("http://api.dotnetgroup.lt") > -1, url);
});

test("given_no_query_provided_getUrl_builds_default_query", function () {
    window.location.hash = "?type=rss";

    var url = Api.getUrl(/*baseUrl*/null, /*query*/null);

    ok(url.indexOf("?type=rss") > -1, url);
});

test("given_base_url_and_query_are_provided_getUrl_uses_both", function () {
    var baseUrl = "http://localhost/";
    var query = $.query.load("#").set("type", "rss");

    var url = Api.getUrl(baseUrl, query);

    equals(url, "http://localhost/?type=rss&callback=?");
});

test("given_query_string_contains_spaces_and_colons_getUrl_build_correct_url", function () {
    window.location.hash = "?from=2012-01-01 08:00:00";

    var url = Api.getUrl();

    ok(url.indexOf("?from=2012-01-01+08:00:00") > -1, url);
});