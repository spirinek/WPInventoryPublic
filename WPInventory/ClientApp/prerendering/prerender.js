const prerendering = require('aspnet-prerendering');

export default prerendering.createServerRenderer(params => {
    return new Promise(function (resolve, reject) {
        resolve({
            // html: result,
            globals: {
                serverInfo: {
                    serverUrl: params.origin,
                    absoluteUrl: params.absoluteUrl,
                    baseUrl: params.baseUrl,
                    authUrl: params.location.auth
                },
                preloadData: params.data
            }
        })
    });
});






