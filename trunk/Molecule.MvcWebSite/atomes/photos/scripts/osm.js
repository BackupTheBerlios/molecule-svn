// load OSM map on element 'id' and center on provided location
function loadMap(id, lat, lon, thumbnail) {
    var zoom = 10

    var map; //complex object of type OpenLayers.Map

    map = new OpenLayers.Map(id, {
        controls: [
                    new OpenLayers.Control.Navigation(),
                    new OpenLayers.Control.PanZoomBar()/*,
                    new OpenLayers.Control.Attribution()*/],
        maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34),
        maxResolution: 156543.0399,
        numZoomLevels: 19,
        units: 'm',
        projection: new OpenLayers.Projection("EPSG:900913"),
        displayProjection: new OpenLayers.Projection("EPSG:4326")
    });


    // map layer
    // layers are OpenLayers.Layer.OSM.Osmarender, OpenLayers.Layer.OSM.Mapnik, OpenLayers.Layer.OSM.Maplint and OpenLayers.Layer.OSM.CycleMap
    layerTilesAtHome = new OpenLayers.Layer.OSM.Mapnik("Mapnik");
    map.addLayer(layerTilesAtHome);

    // layer for photo thumbnail
    layerMarkers = new OpenLayers.Layer.Markers("Markers");
    map.addLayer(layerMarkers);

    // center map
    var lonLat = new OpenLayers.LonLat(lon, lat).transform(new OpenLayers.Projection("EPSG:4326"), map.getProjectionObject());
    map.setCenter(lonLat, zoom);

    // add photo thumbnail
    var sizef = new OpenLayers.Size(86, 90);
    var offsetf = new OpenLayers.Pixel(-(sizef.w / 2), -sizef.h);
    var iconf = new OpenLayers.Icon('images/frame.png', sizef, offsetf);
    layerMarkers.addMarker(new OpenLayers.Marker(lonLat, iconf));
    var size = new OpenLayers.Size(80, 80);
    var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h - 7);
    var icon = new OpenLayers.Icon(thumbnail, size, offset);
    layerMarkers.addMarker(new OpenLayers.Marker(lonLat, icon));

//    var size = new OpenLayers.Size(100, 100);
//    var popup = new OpenLayers.Popup.Framed(
//        'toto',
//        lonLat,
//        size,
//        '<a href="#">coucou</a>',
//        null,
//        true);
//    map.addPopup(popup);
}