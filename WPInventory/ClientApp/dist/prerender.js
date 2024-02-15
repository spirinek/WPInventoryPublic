(function(e, a) { for(var i in a) e[i] = a[i]; }(exports, /******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 1);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

module.exports = require("domain");

/***/ }),
/* 1 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
﻿const prerendering = __webpack_require__(2);

/* harmony default export */ __webpack_exports__["default"] = (prerendering.createServerRenderer(params => {
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
}));








/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(3));


/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var url = __webpack_require__(4);
var domain = __webpack_require__(0);
var main_1 = __webpack_require__(5);
var defaultTimeoutMilliseconds = 30 * 1000;
function createServerRenderer(bootFunc) {
    var resultFunc = function (callback, applicationBasePath, bootModule, absoluteRequestUrl, requestPathAndQuery, customDataParameter, overrideTimeoutMilliseconds, requestPathBase) {
        // Prepare a promise that will represent the completion of all domain tasks in this execution context.
        // The boot code will wait for this before performing its final render.
        var domainTaskCompletionPromiseResolve;
        var domainTaskCompletionPromise = new Promise(function (resolve, reject) {
            domainTaskCompletionPromiseResolve = resolve;
        });
        var parsedAbsoluteRequestUrl = url.parse(absoluteRequestUrl);
        var params = {
            // It's helpful for boot funcs to receive the query as a key-value object, so parse it here
            // e.g., react-redux-router requires location.query to be a key-value object for consistency with client-side behaviour
            location: url.parse(requestPathAndQuery, /* parseQueryString */ true),
            origin: parsedAbsoluteRequestUrl.protocol + '//' + parsedAbsoluteRequestUrl.host,
            url: requestPathAndQuery,
            baseUrl: (requestPathBase || '') + '/',
            absoluteUrl: absoluteRequestUrl,
            domainTasks: domainTaskCompletionPromise,
            data: customDataParameter
        };
        var absoluteBaseUrl = params.origin + params.baseUrl; // Should be same value as page's <base href>
        // Open a new domain that can track all the async tasks involved in the app's execution
        main_1.run(/* code to run */ function () {
            // Workaround for Node bug where native Promise continuations lose their domain context
            // (https://github.com/nodejs/node-v0.x-archive/issues/8648)
            // The domain.active property is set by the domain-context module
            bindPromiseContinuationsToDomain(domainTaskCompletionPromise, domain['active']);
            // Make the base URL available to the 'domain-tasks/fetch' helper within this execution context
            main_1.baseUrl(absoluteBaseUrl);
            // Begin rendering, and apply a timeout
            var bootFuncPromise = bootFunc(params);
            if (!bootFuncPromise || typeof bootFuncPromise.then !== 'function') {
                callback("Prerendering failed because the boot function in " + bootModule.moduleName + " did not return a promise.", null);
                return;
            }
            var timeoutMilliseconds = overrideTimeoutMilliseconds || defaultTimeoutMilliseconds; // e.g., pass -1 to override as 'never time out'
            var bootFuncPromiseWithTimeout = timeoutMilliseconds > 0
                ? wrapWithTimeout(bootFuncPromise, timeoutMilliseconds, "Prerendering timed out after " + timeoutMilliseconds + "ms because the boot function in '" + bootModule.moduleName + "' "
                    + 'returned a promise that did not resolve or reject. Make sure that your boot function always resolves or '
                    + 'rejects its promise. You can change the timeout value using the \'asp-prerender-timeout\' tag helper.')
                : bootFuncPromise;
            // Actually perform the rendering
            bootFuncPromiseWithTimeout.then(function (successResult) {
                callback(null, successResult);
            }, function (error) {
                callback(error, null);
            });
        }, /* completion callback */ function (/* completion callback */ errorOrNothing) {
            if (errorOrNothing) {
                callback(errorOrNothing, null);
            }
            else {
                // There are no more ongoing domain tasks (typically data access operations), so we can resolve
                // the domain tasks promise which notifies the boot code that it can do its final render.
                domainTaskCompletionPromiseResolve();
            }
        });
    };
    // Indicate to the prerendering code bundled into Microsoft.AspNetCore.SpaServices that this is a serverside rendering
    // function, so it can be invoked directly. This flag exists only so that, in its absence, we can run some different
    // backward-compatibility logic.
    resultFunc['isServerRenderer'] = true;
    return resultFunc;
}
exports.createServerRenderer = createServerRenderer;
function wrapWithTimeout(promise, timeoutMilliseconds, timeoutRejectionValue) {
    return new Promise(function (resolve, reject) {
        var timeoutTimer = setTimeout(function () {
            reject(timeoutRejectionValue);
        }, timeoutMilliseconds);
        promise.then(function (resolvedValue) {
            clearTimeout(timeoutTimer);
            resolve(resolvedValue);
        }, function (rejectedValue) {
            clearTimeout(timeoutTimer);
            reject(rejectedValue);
        });
    });
}
function bindPromiseContinuationsToDomain(promise, domainInstance) {
    var originalThen = promise.then;
    promise.then = (function then(resolve, reject) {
        if (typeof resolve === 'function') {
            resolve = domainInstance.bind(resolve);
        }
        if (typeof reject === 'function') {
            reject = domainInstance.bind(reject);
        }
        return originalThen.call(this, resolve, reject);
    });
}


/***/ }),
/* 4 */
/***/ (function(module, exports) {

module.exports = require("url");

/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var domain = __webpack_require__(0);
var domainContext = __webpack_require__(6);
// Not using symbols, because this may need to run in a version of Node.js that doesn't support them
var domainTasksStateKey = '__DOMAIN_TASKS';
var domainTaskBaseUrlStateKey = '__DOMAIN_TASK_INTERNAL_FETCH_BASEURL__DO_NOT_REFERENCE_THIS__';
var noDomainBaseUrl;
function addTask(task) {
    if (task && domain.active) {
        var state_1 = domainContext.get(domainTasksStateKey);
        if (state_1) {
            state_1.numRemainingTasks++;
            task.then(function () {
                // The application may have other listeners chained to this promise *after*
                // this listener, which may in turn register further tasks. Since we don't 
                // want the combined task to complete until all the handlers for child tasks
                // have finished, delay the response to give time for more tasks to be added
                // synchronously.
                setTimeout(function () {
                    state_1.numRemainingTasks--;
                    if (state_1.numRemainingTasks === 0 && !state_1.hasIssuedSuccessCallback) {
                        state_1.hasIssuedSuccessCallback = true;
                        setTimeout(function () {
                            state_1.completionCallback(/* error */ null);
                        }, 0);
                    }
                }, 0);
            }, function (error) {
                state_1.completionCallback(error);
            });
        }
    }
}
exports.addTask = addTask;
function run(codeToRun, completionCallback) {
    var synchronousResult;
    domainContext.runInNewDomain(function () {
        var state = {
            numRemainingTasks: 0,
            hasIssuedSuccessCallback: false,
            completionCallback: domain.active.bind(completionCallback)
        };
        try {
            domainContext.set(domainTasksStateKey, state);
            synchronousResult = codeToRun();
            // If no tasks were registered synchronously, then we're done already
            if (state.numRemainingTasks === 0 && !state.hasIssuedSuccessCallback) {
                state.hasIssuedSuccessCallback = true;
                setTimeout(function () {
                    state.completionCallback(/* error */ null);
                }, 0);
            }
        }
        catch (ex) {
            state.completionCallback(ex);
        }
    });
    return synchronousResult;
}
exports.run = run;
function baseUrl(url) {
    if (url) {
        if (domain.active) {
            // There's an active domain (e.g., in Node.js), so associate the base URL with it
            domainContext.set(domainTaskBaseUrlStateKey, url);
        }
        else {
            // There's no active domain (e.g., in browser), so there's just one shared base URL
            noDomainBaseUrl = url;
        }
    }
    return domain.active ? domainContext.get(domainTaskBaseUrlStateKey) : noDomainBaseUrl;
}
exports.baseUrl = baseUrl;


/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

// Generated by CoffeeScript 1.6.2
var domain;

domain = __webpack_require__(0);

exports.context = function(context, currentDomain) {
  if (currentDomain == null) {
    currentDomain = domain.active;
  }
  if (currentDomain == null) {
    throw new Error('no active domain');
  }
  return currentDomain.__context__ = context != null ? context() : {};
};

exports.cleanup = function(cleanup, context, currentDomain) {
  if (context == null) {
    context = null;
  }
  if (currentDomain == null) {
    currentDomain = domain.active;
  }
  context = context || currentDomain.__context__;
  if ((cleanup != null) && (context != null)) {
    cleanup(context);
  }
  if (currentDomain != null) {
    return currentDomain.__context__ = null;
  }
};

exports.onError = function(err, onError, context, currentDomain) {
  if (context == null) {
    context = null;
  }
  if (currentDomain == null) {
    currentDomain = domain.active;
  }
  context = context || currentDomain.__context__;
  if (onError != null) {
    onError(err, context);
  }
  return currentDomain.__context__ = null;
};

exports.get = function(key, currentDomain) {
  if (currentDomain == null) {
    currentDomain = domain.active;
  }
  if (currentDomain == null) {
    throw new Error('no active domain');
  }
  return currentDomain.__context__[key];
};

exports.set = function(key, value, currentDomain) {
  if (currentDomain == null) {
    currentDomain = domain.active;
  }
  if (currentDomain == null) {
    throw new Error('no active domain');
  }
  return currentDomain.__context__[key] = value;
};

exports.run = function(options, func) {
  var cleanup, context, currentDomain, err, onError;

  if (!func) {
    func = options;
    options = {};
  }
  context = options.context, cleanup = options.cleanup, onError = options.onError;
  currentDomain = options.domain || domain.active;
  if (!currentDomain) {
    throw new Error('no active domain');
  }
  currentDomain.on('dispose', function() {
    return exports.cleanup(cleanup, null, currentDomain);
  });
  currentDomain.on('error', function(err) {
    if (onError != null) {
      return exports.onError(err, onError, null, currentDomain);
    } else {
      return exports.cleanup(cleanup, null, currentDomain);
    }
  });
  exports.context(context, currentDomain);
  try {
    currentDomain.bind(func, true)();
  } catch (_error) {
    err = _error;
    currentDomain.emit('error', err);
  }
  return currentDomain;
};

exports.runInNewDomain = function(options, func) {
  var currentDomain;

  if (!func) {
    func = options;
    options = {};
  }
  currentDomain = domain.active;
  options.domain = domain.create();
  if (!options.detach && currentDomain) {
    currentDomain.add(options.domain);
    options.domain.on('error', function(err) {
      return currentDomain.emit('error', err);
    });
    currentDomain.on('dispose', function() {
      return options.domain.dispose();
    });
  }
  return exports.run(options, func);
};

exports.middleware = function(context, cleanup) {
  return function(req, res, next) {
    var currentDomain, _ref;

    if (typeof context !== 'function') {
      _ref = context, context = _ref.context, cleanup = _ref.cleanup;
    }
    currentDomain = domain.active;
    exports.context(context, currentDomain);
    res.on('finish', function() {
      return exports.cleanup(cleanup, null, currentDomain);
    });
    req.__context__ = currentDomain.__context__;
    return next();
  };
};

exports.middlewareOnError = function(onError) {
  return function(err, req, res, next) {
    if (typeof onError !== 'function') {
      onError = onError.onError;
    }
    if (onError != null) {
      exports.onError(err, onError, req.__context__);
    } else {
      exports.cleanup(onError, req.__context__);
    }
    req.__context__ = null;
    return next(err);
  };
};


/***/ })
/******/ ])));
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly8vd2VicGFjay9ib290c3RyYXAgNTJlMzIwNzcxMDY5ZjJiZWRmYmYiLCJ3ZWJwYWNrOi8vL2V4dGVybmFsIFwiZG9tYWluXCIiLCJ3ZWJwYWNrOi8vLy4vQ2xpZW50QXBwL3ByZXJlbmRlcmluZy9wcmVyZW5kZXIuanMiLCJ3ZWJwYWNrOi8vLy4vbm9kZV9tb2R1bGVzL2FzcG5ldC1wcmVyZW5kZXJpbmcvaW5kZXguanMiLCJ3ZWJwYWNrOi8vLy4vbm9kZV9tb2R1bGVzL2FzcG5ldC1wcmVyZW5kZXJpbmcvUHJlcmVuZGVyaW5nLmpzIiwid2VicGFjazovLy9leHRlcm5hbCBcInVybFwiIiwid2VicGFjazovLy8uL25vZGVfbW9kdWxlcy9kb21haW4tdGFzay9tYWluLmpzIiwid2VicGFjazovLy8uL25vZGVfbW9kdWxlcy9kb21haW4tY29udGV4dC9saWIvaW5kZXguanMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IjtRQUFBO1FBQ0E7O1FBRUE7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBOzs7UUFHQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQSxLQUFLO1FBQ0w7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQSwyQkFBMkIsMEJBQTBCLEVBQUU7UUFDdkQsaUNBQWlDLGVBQWU7UUFDaEQ7UUFDQTtRQUNBOztRQUVBO1FBQ0Esc0RBQXNELCtEQUErRDs7UUFFckg7UUFDQTs7UUFFQTtRQUNBOzs7Ozs7O0FDN0RBLG1DOzs7Ozs7O0FDQUE7QUFBQSxzQkFBc0IsbUJBQU8sQ0FBQyxDQUFxQjs7QUFFcEM7QUFDZjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpQkFBaUI7QUFDakI7QUFDQTtBQUNBLFNBQVM7QUFDVCxLQUFLO0FBQ0wsQ0FBQyxDQUFDLEVBQUM7Ozs7Ozs7Ozs7Ozs7O0FDakJVO0FBQ2I7QUFDQTtBQUNBO0FBQ0EsOENBQThDLGNBQWM7QUFDNUQsU0FBUyxtQkFBTyxDQUFDLENBQWdCOzs7Ozs7OztBQ0xwQjtBQUNiLDhDQUE4QyxjQUFjO0FBQzVELFVBQVUsbUJBQU8sQ0FBQyxDQUFLO0FBQ3ZCLGFBQWEsbUJBQU8sQ0FBQyxDQUFRO0FBQzdCLGFBQWEsbUJBQU8sQ0FBQyxDQUFrQjtBQUN2QztBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDZEQUE2RDtBQUM3RDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsZ0dBQWdHO0FBQ2hHO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxhQUFhO0FBQ2I7QUFDQSxhQUFhO0FBQ2IsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0EsU0FBUztBQUNULEtBQUs7QUFDTDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsS0FBSztBQUNMOzs7Ozs7O0FDaEdBLGdDOzs7Ozs7O0FDQWE7QUFDYiw4Q0FBOEMsY0FBYztBQUM1RCxhQUFhLG1CQUFPLENBQUMsQ0FBUTtBQUM3QixvQkFBb0IsbUJBQU8sQ0FBQyxDQUFnQjtBQUM1QztBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSx5QkFBeUI7QUFDekI7QUFDQSxpQkFBaUI7QUFDakIsYUFBYTtBQUNiO0FBQ0EsYUFBYTtBQUNiO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpQkFBaUI7QUFDakI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEtBQUs7QUFDTDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7Ozs7O0FDMUVBO0FBQ0E7O0FBRUEsU0FBUyxtQkFBTyxDQUFDLENBQVE7O0FBRXpCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTtBQUNBO0FBQ0EsS0FBSztBQUNMO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTtBQUNBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEtBQUs7QUFDTDtBQUNBO0FBQ0EsS0FBSztBQUNMO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxLQUFLO0FBQ0w7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxLQUFLO0FBQ0w7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBIiwiZmlsZSI6InByZXJlbmRlci5qcyIsInNvdXJjZXNDb250ZW50IjpbIiBcdC8vIFRoZSBtb2R1bGUgY2FjaGVcbiBcdHZhciBpbnN0YWxsZWRNb2R1bGVzID0ge307XG5cbiBcdC8vIFRoZSByZXF1aXJlIGZ1bmN0aW9uXG4gXHRmdW5jdGlvbiBfX3dlYnBhY2tfcmVxdWlyZV9fKG1vZHVsZUlkKSB7XG5cbiBcdFx0Ly8gQ2hlY2sgaWYgbW9kdWxlIGlzIGluIGNhY2hlXG4gXHRcdGlmKGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdKSB7XG4gXHRcdFx0cmV0dXJuIGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdLmV4cG9ydHM7XG4gXHRcdH1cbiBcdFx0Ly8gQ3JlYXRlIGEgbmV3IG1vZHVsZSAoYW5kIHB1dCBpdCBpbnRvIHRoZSBjYWNoZSlcbiBcdFx0dmFyIG1vZHVsZSA9IGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdID0ge1xuIFx0XHRcdGk6IG1vZHVsZUlkLFxuIFx0XHRcdGw6IGZhbHNlLFxuIFx0XHRcdGV4cG9ydHM6IHt9XG4gXHRcdH07XG5cbiBcdFx0Ly8gRXhlY3V0ZSB0aGUgbW9kdWxlIGZ1bmN0aW9uXG4gXHRcdG1vZHVsZXNbbW9kdWxlSWRdLmNhbGwobW9kdWxlLmV4cG9ydHMsIG1vZHVsZSwgbW9kdWxlLmV4cG9ydHMsIF9fd2VicGFja19yZXF1aXJlX18pO1xuXG4gXHRcdC8vIEZsYWcgdGhlIG1vZHVsZSBhcyBsb2FkZWRcbiBcdFx0bW9kdWxlLmwgPSB0cnVlO1xuXG4gXHRcdC8vIFJldHVybiB0aGUgZXhwb3J0cyBvZiB0aGUgbW9kdWxlXG4gXHRcdHJldHVybiBtb2R1bGUuZXhwb3J0cztcbiBcdH1cblxuXG4gXHQvLyBleHBvc2UgdGhlIG1vZHVsZXMgb2JqZWN0IChfX3dlYnBhY2tfbW9kdWxlc19fKVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5tID0gbW9kdWxlcztcblxuIFx0Ly8gZXhwb3NlIHRoZSBtb2R1bGUgY2FjaGVcbiBcdF9fd2VicGFja19yZXF1aXJlX18uYyA9IGluc3RhbGxlZE1vZHVsZXM7XG5cbiBcdC8vIGRlZmluZSBnZXR0ZXIgZnVuY3Rpb24gZm9yIGhhcm1vbnkgZXhwb3J0c1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kID0gZnVuY3Rpb24oZXhwb3J0cywgbmFtZSwgZ2V0dGVyKSB7XG4gXHRcdGlmKCFfX3dlYnBhY2tfcmVxdWlyZV9fLm8oZXhwb3J0cywgbmFtZSkpIHtcbiBcdFx0XHRPYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgbmFtZSwge1xuIFx0XHRcdFx0Y29uZmlndXJhYmxlOiBmYWxzZSxcbiBcdFx0XHRcdGVudW1lcmFibGU6IHRydWUsXG4gXHRcdFx0XHRnZXQ6IGdldHRlclxuIFx0XHRcdH0pO1xuIFx0XHR9XG4gXHR9O1xuXG4gXHQvLyBnZXREZWZhdWx0RXhwb3J0IGZ1bmN0aW9uIGZvciBjb21wYXRpYmlsaXR5IHdpdGggbm9uLWhhcm1vbnkgbW9kdWxlc1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5uID0gZnVuY3Rpb24obW9kdWxlKSB7XG4gXHRcdHZhciBnZXR0ZXIgPSBtb2R1bGUgJiYgbW9kdWxlLl9fZXNNb2R1bGUgP1xuIFx0XHRcdGZ1bmN0aW9uIGdldERlZmF1bHQoKSB7IHJldHVybiBtb2R1bGVbJ2RlZmF1bHQnXTsgfSA6XG4gXHRcdFx0ZnVuY3Rpb24gZ2V0TW9kdWxlRXhwb3J0cygpIHsgcmV0dXJuIG1vZHVsZTsgfTtcbiBcdFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kKGdldHRlciwgJ2EnLCBnZXR0ZXIpO1xuIFx0XHRyZXR1cm4gZ2V0dGVyO1xuIFx0fTtcblxuIFx0Ly8gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm8gPSBmdW5jdGlvbihvYmplY3QsIHByb3BlcnR5KSB7IHJldHVybiBPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5LmNhbGwob2JqZWN0LCBwcm9wZXJ0eSk7IH07XG5cbiBcdC8vIF9fd2VicGFja19wdWJsaWNfcGF0aF9fXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnAgPSBcIlwiO1xuXG4gXHQvLyBMb2FkIGVudHJ5IG1vZHVsZSBhbmQgcmV0dXJuIGV4cG9ydHNcbiBcdHJldHVybiBfX3dlYnBhY2tfcmVxdWlyZV9fKF9fd2VicGFja19yZXF1aXJlX18ucyA9IDEpO1xuXG5cblxuLy8gV0VCUEFDSyBGT09URVIgLy9cbi8vIHdlYnBhY2svYm9vdHN0cmFwIDUyZTMyMDc3MTA2OWYyYmVkZmJmIiwibW9kdWxlLmV4cG9ydHMgPSByZXF1aXJlKFwiZG9tYWluXCIpO1xuXG5cbi8vLy8vLy8vLy8vLy8vLy8vL1xuLy8gV0VCUEFDSyBGT09URVJcbi8vIGV4dGVybmFsIFwiZG9tYWluXCJcbi8vIG1vZHVsZSBpZCA9IDBcbi8vIG1vZHVsZSBjaHVua3MgPSAwIiwi77u/Y29uc3QgcHJlcmVuZGVyaW5nID0gcmVxdWlyZSgnYXNwbmV0LXByZXJlbmRlcmluZycpO1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgcHJlcmVuZGVyaW5nLmNyZWF0ZVNlcnZlclJlbmRlcmVyKHBhcmFtcyA9PiB7XHJcbiAgICByZXR1cm4gbmV3IFByb21pc2UoZnVuY3Rpb24gKHJlc29sdmUsIHJlamVjdCkge1xyXG4gICAgICAgIHJlc29sdmUoe1xyXG4gICAgICAgICAgICAvLyBodG1sOiByZXN1bHQsXHJcbiAgICAgICAgICAgIGdsb2JhbHM6IHtcclxuICAgICAgICAgICAgICAgIHNlcnZlckluZm86IHtcclxuICAgICAgICAgICAgICAgICAgICBzZXJ2ZXJVcmw6IHBhcmFtcy5vcmlnaW4sXHJcbiAgICAgICAgICAgICAgICAgICAgYWJzb2x1dGVVcmw6IHBhcmFtcy5hYnNvbHV0ZVVybCxcclxuICAgICAgICAgICAgICAgICAgICBiYXNlVXJsOiBwYXJhbXMuYmFzZVVybCxcclxuICAgICAgICAgICAgICAgICAgICBhdXRoVXJsOiBwYXJhbXMubG9jYXRpb24uYXV0aFxyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHByZWxvYWREYXRhOiBwYXJhbXMuZGF0YVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSlcclxuICAgIH0pO1xyXG59KTtcclxuXHJcblxyXG5cclxuXHJcblxyXG5cclxuXG5cblxuLy8vLy8vLy8vLy8vLy8vLy8vXG4vLyBXRUJQQUNLIEZPT1RFUlxuLy8gLi9DbGllbnRBcHAvcHJlcmVuZGVyaW5nL3ByZXJlbmRlci5qc1xuLy8gbW9kdWxlIGlkID0gMVxuLy8gbW9kdWxlIGNodW5rcyA9IDAiLCJcInVzZSBzdHJpY3RcIjtcclxuZnVuY3Rpb24gX19leHBvcnQobSkge1xyXG4gICAgZm9yICh2YXIgcCBpbiBtKSBpZiAoIWV4cG9ydHMuaGFzT3duUHJvcGVydHkocCkpIGV4cG9ydHNbcF0gPSBtW3BdO1xyXG59XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuX19leHBvcnQocmVxdWlyZShcIi4vUHJlcmVuZGVyaW5nXCIpKTtcclxuXG5cblxuLy8vLy8vLy8vLy8vLy8vLy8vXG4vLyBXRUJQQUNLIEZPT1RFUlxuLy8gLi9ub2RlX21vZHVsZXMvYXNwbmV0LXByZXJlbmRlcmluZy9pbmRleC5qc1xuLy8gbW9kdWxlIGlkID0gMlxuLy8gbW9kdWxlIGNodW5rcyA9IDAiLCJcInVzZSBzdHJpY3RcIjtcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG52YXIgdXJsID0gcmVxdWlyZShcInVybFwiKTtcclxudmFyIGRvbWFpbiA9IHJlcXVpcmUoXCJkb21haW5cIik7XHJcbnZhciBtYWluXzEgPSByZXF1aXJlKFwiZG9tYWluLXRhc2svbWFpblwiKTtcclxudmFyIGRlZmF1bHRUaW1lb3V0TWlsbGlzZWNvbmRzID0gMzAgKiAxMDAwO1xyXG5mdW5jdGlvbiBjcmVhdGVTZXJ2ZXJSZW5kZXJlcihib290RnVuYykge1xyXG4gICAgdmFyIHJlc3VsdEZ1bmMgPSBmdW5jdGlvbiAoY2FsbGJhY2ssIGFwcGxpY2F0aW9uQmFzZVBhdGgsIGJvb3RNb2R1bGUsIGFic29sdXRlUmVxdWVzdFVybCwgcmVxdWVzdFBhdGhBbmRRdWVyeSwgY3VzdG9tRGF0YVBhcmFtZXRlciwgb3ZlcnJpZGVUaW1lb3V0TWlsbGlzZWNvbmRzLCByZXF1ZXN0UGF0aEJhc2UpIHtcclxuICAgICAgICAvLyBQcmVwYXJlIGEgcHJvbWlzZSB0aGF0IHdpbGwgcmVwcmVzZW50IHRoZSBjb21wbGV0aW9uIG9mIGFsbCBkb21haW4gdGFza3MgaW4gdGhpcyBleGVjdXRpb24gY29udGV4dC5cclxuICAgICAgICAvLyBUaGUgYm9vdCBjb2RlIHdpbGwgd2FpdCBmb3IgdGhpcyBiZWZvcmUgcGVyZm9ybWluZyBpdHMgZmluYWwgcmVuZGVyLlxyXG4gICAgICAgIHZhciBkb21haW5UYXNrQ29tcGxldGlvblByb21pc2VSZXNvbHZlO1xyXG4gICAgICAgIHZhciBkb21haW5UYXNrQ29tcGxldGlvblByb21pc2UgPSBuZXcgUHJvbWlzZShmdW5jdGlvbiAocmVzb2x2ZSwgcmVqZWN0KSB7XHJcbiAgICAgICAgICAgIGRvbWFpblRhc2tDb21wbGV0aW9uUHJvbWlzZVJlc29sdmUgPSByZXNvbHZlO1xyXG4gICAgICAgIH0pO1xyXG4gICAgICAgIHZhciBwYXJzZWRBYnNvbHV0ZVJlcXVlc3RVcmwgPSB1cmwucGFyc2UoYWJzb2x1dGVSZXF1ZXN0VXJsKTtcclxuICAgICAgICB2YXIgcGFyYW1zID0ge1xyXG4gICAgICAgICAgICAvLyBJdCdzIGhlbHBmdWwgZm9yIGJvb3QgZnVuY3MgdG8gcmVjZWl2ZSB0aGUgcXVlcnkgYXMgYSBrZXktdmFsdWUgb2JqZWN0LCBzbyBwYXJzZSBpdCBoZXJlXHJcbiAgICAgICAgICAgIC8vIGUuZy4sIHJlYWN0LXJlZHV4LXJvdXRlciByZXF1aXJlcyBsb2NhdGlvbi5xdWVyeSB0byBiZSBhIGtleS12YWx1ZSBvYmplY3QgZm9yIGNvbnNpc3RlbmN5IHdpdGggY2xpZW50LXNpZGUgYmVoYXZpb3VyXHJcbiAgICAgICAgICAgIGxvY2F0aW9uOiB1cmwucGFyc2UocmVxdWVzdFBhdGhBbmRRdWVyeSwgLyogcGFyc2VRdWVyeVN0cmluZyAqLyB0cnVlKSxcclxuICAgICAgICAgICAgb3JpZ2luOiBwYXJzZWRBYnNvbHV0ZVJlcXVlc3RVcmwucHJvdG9jb2wgKyAnLy8nICsgcGFyc2VkQWJzb2x1dGVSZXF1ZXN0VXJsLmhvc3QsXHJcbiAgICAgICAgICAgIHVybDogcmVxdWVzdFBhdGhBbmRRdWVyeSxcclxuICAgICAgICAgICAgYmFzZVVybDogKHJlcXVlc3RQYXRoQmFzZSB8fCAnJykgKyAnLycsXHJcbiAgICAgICAgICAgIGFic29sdXRlVXJsOiBhYnNvbHV0ZVJlcXVlc3RVcmwsXHJcbiAgICAgICAgICAgIGRvbWFpblRhc2tzOiBkb21haW5UYXNrQ29tcGxldGlvblByb21pc2UsXHJcbiAgICAgICAgICAgIGRhdGE6IGN1c3RvbURhdGFQYXJhbWV0ZXJcclxuICAgICAgICB9O1xyXG4gICAgICAgIHZhciBhYnNvbHV0ZUJhc2VVcmwgPSBwYXJhbXMub3JpZ2luICsgcGFyYW1zLmJhc2VVcmw7IC8vIFNob3VsZCBiZSBzYW1lIHZhbHVlIGFzIHBhZ2UncyA8YmFzZSBocmVmPlxyXG4gICAgICAgIC8vIE9wZW4gYSBuZXcgZG9tYWluIHRoYXQgY2FuIHRyYWNrIGFsbCB0aGUgYXN5bmMgdGFza3MgaW52b2x2ZWQgaW4gdGhlIGFwcCdzIGV4ZWN1dGlvblxyXG4gICAgICAgIG1haW5fMS5ydW4oLyogY29kZSB0byBydW4gKi8gZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICAvLyBXb3JrYXJvdW5kIGZvciBOb2RlIGJ1ZyB3aGVyZSBuYXRpdmUgUHJvbWlzZSBjb250aW51YXRpb25zIGxvc2UgdGhlaXIgZG9tYWluIGNvbnRleHRcclxuICAgICAgICAgICAgLy8gKGh0dHBzOi8vZ2l0aHViLmNvbS9ub2RlanMvbm9kZS12MC54LWFyY2hpdmUvaXNzdWVzLzg2NDgpXHJcbiAgICAgICAgICAgIC8vIFRoZSBkb21haW4uYWN0aXZlIHByb3BlcnR5IGlzIHNldCBieSB0aGUgZG9tYWluLWNvbnRleHQgbW9kdWxlXHJcbiAgICAgICAgICAgIGJpbmRQcm9taXNlQ29udGludWF0aW9uc1RvRG9tYWluKGRvbWFpblRhc2tDb21wbGV0aW9uUHJvbWlzZSwgZG9tYWluWydhY3RpdmUnXSk7XHJcbiAgICAgICAgICAgIC8vIE1ha2UgdGhlIGJhc2UgVVJMIGF2YWlsYWJsZSB0byB0aGUgJ2RvbWFpbi10YXNrcy9mZXRjaCcgaGVscGVyIHdpdGhpbiB0aGlzIGV4ZWN1dGlvbiBjb250ZXh0XHJcbiAgICAgICAgICAgIG1haW5fMS5iYXNlVXJsKGFic29sdXRlQmFzZVVybCk7XHJcbiAgICAgICAgICAgIC8vIEJlZ2luIHJlbmRlcmluZywgYW5kIGFwcGx5IGEgdGltZW91dFxyXG4gICAgICAgICAgICB2YXIgYm9vdEZ1bmNQcm9taXNlID0gYm9vdEZ1bmMocGFyYW1zKTtcclxuICAgICAgICAgICAgaWYgKCFib290RnVuY1Byb21pc2UgfHwgdHlwZW9mIGJvb3RGdW5jUHJvbWlzZS50aGVuICE9PSAnZnVuY3Rpb24nKSB7XHJcbiAgICAgICAgICAgICAgICBjYWxsYmFjayhcIlByZXJlbmRlcmluZyBmYWlsZWQgYmVjYXVzZSB0aGUgYm9vdCBmdW5jdGlvbiBpbiBcIiArIGJvb3RNb2R1bGUubW9kdWxlTmFtZSArIFwiIGRpZCBub3QgcmV0dXJuIGEgcHJvbWlzZS5cIiwgbnVsbCk7XHJcbiAgICAgICAgICAgICAgICByZXR1cm47XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgdmFyIHRpbWVvdXRNaWxsaXNlY29uZHMgPSBvdmVycmlkZVRpbWVvdXRNaWxsaXNlY29uZHMgfHwgZGVmYXVsdFRpbWVvdXRNaWxsaXNlY29uZHM7IC8vIGUuZy4sIHBhc3MgLTEgdG8gb3ZlcnJpZGUgYXMgJ25ldmVyIHRpbWUgb3V0J1xyXG4gICAgICAgICAgICB2YXIgYm9vdEZ1bmNQcm9taXNlV2l0aFRpbWVvdXQgPSB0aW1lb3V0TWlsbGlzZWNvbmRzID4gMFxyXG4gICAgICAgICAgICAgICAgPyB3cmFwV2l0aFRpbWVvdXQoYm9vdEZ1bmNQcm9taXNlLCB0aW1lb3V0TWlsbGlzZWNvbmRzLCBcIlByZXJlbmRlcmluZyB0aW1lZCBvdXQgYWZ0ZXIgXCIgKyB0aW1lb3V0TWlsbGlzZWNvbmRzICsgXCJtcyBiZWNhdXNlIHRoZSBib290IGZ1bmN0aW9uIGluICdcIiArIGJvb3RNb2R1bGUubW9kdWxlTmFtZSArIFwiJyBcIlxyXG4gICAgICAgICAgICAgICAgICAgICsgJ3JldHVybmVkIGEgcHJvbWlzZSB0aGF0IGRpZCBub3QgcmVzb2x2ZSBvciByZWplY3QuIE1ha2Ugc3VyZSB0aGF0IHlvdXIgYm9vdCBmdW5jdGlvbiBhbHdheXMgcmVzb2x2ZXMgb3IgJ1xyXG4gICAgICAgICAgICAgICAgICAgICsgJ3JlamVjdHMgaXRzIHByb21pc2UuIFlvdSBjYW4gY2hhbmdlIHRoZSB0aW1lb3V0IHZhbHVlIHVzaW5nIHRoZSBcXCdhc3AtcHJlcmVuZGVyLXRpbWVvdXRcXCcgdGFnIGhlbHBlci4nKVxyXG4gICAgICAgICAgICAgICAgOiBib290RnVuY1Byb21pc2U7XHJcbiAgICAgICAgICAgIC8vIEFjdHVhbGx5IHBlcmZvcm0gdGhlIHJlbmRlcmluZ1xyXG4gICAgICAgICAgICBib290RnVuY1Byb21pc2VXaXRoVGltZW91dC50aGVuKGZ1bmN0aW9uIChzdWNjZXNzUmVzdWx0KSB7XHJcbiAgICAgICAgICAgICAgICBjYWxsYmFjayhudWxsLCBzdWNjZXNzUmVzdWx0KTtcclxuICAgICAgICAgICAgfSwgZnVuY3Rpb24gKGVycm9yKSB7XHJcbiAgICAgICAgICAgICAgICBjYWxsYmFjayhlcnJvciwgbnVsbCk7XHJcbiAgICAgICAgICAgIH0pO1xyXG4gICAgICAgIH0sIC8qIGNvbXBsZXRpb24gY2FsbGJhY2sgKi8gZnVuY3Rpb24gKC8qIGNvbXBsZXRpb24gY2FsbGJhY2sgKi8gZXJyb3JPck5vdGhpbmcpIHtcclxuICAgICAgICAgICAgaWYgKGVycm9yT3JOb3RoaW5nKSB7XHJcbiAgICAgICAgICAgICAgICBjYWxsYmFjayhlcnJvck9yTm90aGluZywgbnVsbCk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAvLyBUaGVyZSBhcmUgbm8gbW9yZSBvbmdvaW5nIGRvbWFpbiB0YXNrcyAodHlwaWNhbGx5IGRhdGEgYWNjZXNzIG9wZXJhdGlvbnMpLCBzbyB3ZSBjYW4gcmVzb2x2ZVxyXG4gICAgICAgICAgICAgICAgLy8gdGhlIGRvbWFpbiB0YXNrcyBwcm9taXNlIHdoaWNoIG5vdGlmaWVzIHRoZSBib290IGNvZGUgdGhhdCBpdCBjYW4gZG8gaXRzIGZpbmFsIHJlbmRlci5cclxuICAgICAgICAgICAgICAgIGRvbWFpblRhc2tDb21wbGV0aW9uUHJvbWlzZVJlc29sdmUoKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH0pO1xyXG4gICAgfTtcclxuICAgIC8vIEluZGljYXRlIHRvIHRoZSBwcmVyZW5kZXJpbmcgY29kZSBidW5kbGVkIGludG8gTWljcm9zb2Z0LkFzcE5ldENvcmUuU3BhU2VydmljZXMgdGhhdCB0aGlzIGlzIGEgc2VydmVyc2lkZSByZW5kZXJpbmdcclxuICAgIC8vIGZ1bmN0aW9uLCBzbyBpdCBjYW4gYmUgaW52b2tlZCBkaXJlY3RseS4gVGhpcyBmbGFnIGV4aXN0cyBvbmx5IHNvIHRoYXQsIGluIGl0cyBhYnNlbmNlLCB3ZSBjYW4gcnVuIHNvbWUgZGlmZmVyZW50XHJcbiAgICAvLyBiYWNrd2FyZC1jb21wYXRpYmlsaXR5IGxvZ2ljLlxyXG4gICAgcmVzdWx0RnVuY1snaXNTZXJ2ZXJSZW5kZXJlciddID0gdHJ1ZTtcclxuICAgIHJldHVybiByZXN1bHRGdW5jO1xyXG59XHJcbmV4cG9ydHMuY3JlYXRlU2VydmVyUmVuZGVyZXIgPSBjcmVhdGVTZXJ2ZXJSZW5kZXJlcjtcclxuZnVuY3Rpb24gd3JhcFdpdGhUaW1lb3V0KHByb21pc2UsIHRpbWVvdXRNaWxsaXNlY29uZHMsIHRpbWVvdXRSZWplY3Rpb25WYWx1ZSkge1xyXG4gICAgcmV0dXJuIG5ldyBQcm9taXNlKGZ1bmN0aW9uIChyZXNvbHZlLCByZWplY3QpIHtcclxuICAgICAgICB2YXIgdGltZW91dFRpbWVyID0gc2V0VGltZW91dChmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgIHJlamVjdCh0aW1lb3V0UmVqZWN0aW9uVmFsdWUpO1xyXG4gICAgICAgIH0sIHRpbWVvdXRNaWxsaXNlY29uZHMpO1xyXG4gICAgICAgIHByb21pc2UudGhlbihmdW5jdGlvbiAocmVzb2x2ZWRWYWx1ZSkge1xyXG4gICAgICAgICAgICBjbGVhclRpbWVvdXQodGltZW91dFRpbWVyKTtcclxuICAgICAgICAgICAgcmVzb2x2ZShyZXNvbHZlZFZhbHVlKTtcclxuICAgICAgICB9LCBmdW5jdGlvbiAocmVqZWN0ZWRWYWx1ZSkge1xyXG4gICAgICAgICAgICBjbGVhclRpbWVvdXQodGltZW91dFRpbWVyKTtcclxuICAgICAgICAgICAgcmVqZWN0KHJlamVjdGVkVmFsdWUpO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfSk7XHJcbn1cclxuZnVuY3Rpb24gYmluZFByb21pc2VDb250aW51YXRpb25zVG9Eb21haW4ocHJvbWlzZSwgZG9tYWluSW5zdGFuY2UpIHtcclxuICAgIHZhciBvcmlnaW5hbFRoZW4gPSBwcm9taXNlLnRoZW47XHJcbiAgICBwcm9taXNlLnRoZW4gPSAoZnVuY3Rpb24gdGhlbihyZXNvbHZlLCByZWplY3QpIHtcclxuICAgICAgICBpZiAodHlwZW9mIHJlc29sdmUgPT09ICdmdW5jdGlvbicpIHtcclxuICAgICAgICAgICAgcmVzb2x2ZSA9IGRvbWFpbkluc3RhbmNlLmJpbmQocmVzb2x2ZSk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmICh0eXBlb2YgcmVqZWN0ID09PSAnZnVuY3Rpb24nKSB7XHJcbiAgICAgICAgICAgIHJlamVjdCA9IGRvbWFpbkluc3RhbmNlLmJpbmQocmVqZWN0KTtcclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIG9yaWdpbmFsVGhlbi5jYWxsKHRoaXMsIHJlc29sdmUsIHJlamVjdCk7XHJcbiAgICB9KTtcclxufVxyXG5cblxuXG4vLy8vLy8vLy8vLy8vLy8vLy9cbi8vIFdFQlBBQ0sgRk9PVEVSXG4vLyAuL25vZGVfbW9kdWxlcy9hc3BuZXQtcHJlcmVuZGVyaW5nL1ByZXJlbmRlcmluZy5qc1xuLy8gbW9kdWxlIGlkID0gM1xuLy8gbW9kdWxlIGNodW5rcyA9IDAiLCJtb2R1bGUuZXhwb3J0cyA9IHJlcXVpcmUoXCJ1cmxcIik7XG5cblxuLy8vLy8vLy8vLy8vLy8vLy8vXG4vLyBXRUJQQUNLIEZPT1RFUlxuLy8gZXh0ZXJuYWwgXCJ1cmxcIlxuLy8gbW9kdWxlIGlkID0gNFxuLy8gbW9kdWxlIGNodW5rcyA9IDAiLCJcInVzZSBzdHJpY3RcIjtcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG52YXIgZG9tYWluID0gcmVxdWlyZShcImRvbWFpblwiKTtcclxudmFyIGRvbWFpbkNvbnRleHQgPSByZXF1aXJlKFwiZG9tYWluLWNvbnRleHRcIik7XHJcbi8vIE5vdCB1c2luZyBzeW1ib2xzLCBiZWNhdXNlIHRoaXMgbWF5IG5lZWQgdG8gcnVuIGluIGEgdmVyc2lvbiBvZiBOb2RlLmpzIHRoYXQgZG9lc24ndCBzdXBwb3J0IHRoZW1cclxudmFyIGRvbWFpblRhc2tzU3RhdGVLZXkgPSAnX19ET01BSU5fVEFTS1MnO1xyXG52YXIgZG9tYWluVGFza0Jhc2VVcmxTdGF0ZUtleSA9ICdfX0RPTUFJTl9UQVNLX0lOVEVSTkFMX0ZFVENIX0JBU0VVUkxfX0RPX05PVF9SRUZFUkVOQ0VfVEhJU19fJztcclxudmFyIG5vRG9tYWluQmFzZVVybDtcclxuZnVuY3Rpb24gYWRkVGFzayh0YXNrKSB7XHJcbiAgICBpZiAodGFzayAmJiBkb21haW4uYWN0aXZlKSB7XHJcbiAgICAgICAgdmFyIHN0YXRlXzEgPSBkb21haW5Db250ZXh0LmdldChkb21haW5UYXNrc1N0YXRlS2V5KTtcclxuICAgICAgICBpZiAoc3RhdGVfMSkge1xyXG4gICAgICAgICAgICBzdGF0ZV8xLm51bVJlbWFpbmluZ1Rhc2tzKys7XHJcbiAgICAgICAgICAgIHRhc2sudGhlbihmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgICAgICAvLyBUaGUgYXBwbGljYXRpb24gbWF5IGhhdmUgb3RoZXIgbGlzdGVuZXJzIGNoYWluZWQgdG8gdGhpcyBwcm9taXNlICphZnRlcipcclxuICAgICAgICAgICAgICAgIC8vIHRoaXMgbGlzdGVuZXIsIHdoaWNoIG1heSBpbiB0dXJuIHJlZ2lzdGVyIGZ1cnRoZXIgdGFza3MuIFNpbmNlIHdlIGRvbid0IFxyXG4gICAgICAgICAgICAgICAgLy8gd2FudCB0aGUgY29tYmluZWQgdGFzayB0byBjb21wbGV0ZSB1bnRpbCBhbGwgdGhlIGhhbmRsZXJzIGZvciBjaGlsZCB0YXNrc1xyXG4gICAgICAgICAgICAgICAgLy8gaGF2ZSBmaW5pc2hlZCwgZGVsYXkgdGhlIHJlc3BvbnNlIHRvIGdpdmUgdGltZSBmb3IgbW9yZSB0YXNrcyB0byBiZSBhZGRlZFxyXG4gICAgICAgICAgICAgICAgLy8gc3luY2hyb25vdXNseS5cclxuICAgICAgICAgICAgICAgIHNldFRpbWVvdXQoZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICAgICAgICAgIHN0YXRlXzEubnVtUmVtYWluaW5nVGFza3MtLTtcclxuICAgICAgICAgICAgICAgICAgICBpZiAoc3RhdGVfMS5udW1SZW1haW5pbmdUYXNrcyA9PT0gMCAmJiAhc3RhdGVfMS5oYXNJc3N1ZWRTdWNjZXNzQ2FsbGJhY2spIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgc3RhdGVfMS5oYXNJc3N1ZWRTdWNjZXNzQ2FsbGJhY2sgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBzZXRUaW1lb3V0KGZ1bmN0aW9uICgpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHN0YXRlXzEuY29tcGxldGlvbkNhbGxiYWNrKC8qIGVycm9yICovIG51bGwpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9LCAwKTtcclxuICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICB9LCAwKTtcclxuICAgICAgICAgICAgfSwgZnVuY3Rpb24gKGVycm9yKSB7XHJcbiAgICAgICAgICAgICAgICBzdGF0ZV8xLmNvbXBsZXRpb25DYWxsYmFjayhlcnJvcik7XHJcbiAgICAgICAgICAgIH0pO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufVxyXG5leHBvcnRzLmFkZFRhc2sgPSBhZGRUYXNrO1xyXG5mdW5jdGlvbiBydW4oY29kZVRvUnVuLCBjb21wbGV0aW9uQ2FsbGJhY2spIHtcclxuICAgIHZhciBzeW5jaHJvbm91c1Jlc3VsdDtcclxuICAgIGRvbWFpbkNvbnRleHQucnVuSW5OZXdEb21haW4oZnVuY3Rpb24gKCkge1xyXG4gICAgICAgIHZhciBzdGF0ZSA9IHtcclxuICAgICAgICAgICAgbnVtUmVtYWluaW5nVGFza3M6IDAsXHJcbiAgICAgICAgICAgIGhhc0lzc3VlZFN1Y2Nlc3NDYWxsYmFjazogZmFsc2UsXHJcbiAgICAgICAgICAgIGNvbXBsZXRpb25DYWxsYmFjazogZG9tYWluLmFjdGl2ZS5iaW5kKGNvbXBsZXRpb25DYWxsYmFjaylcclxuICAgICAgICB9O1xyXG4gICAgICAgIHRyeSB7XHJcbiAgICAgICAgICAgIGRvbWFpbkNvbnRleHQuc2V0KGRvbWFpblRhc2tzU3RhdGVLZXksIHN0YXRlKTtcclxuICAgICAgICAgICAgc3luY2hyb25vdXNSZXN1bHQgPSBjb2RlVG9SdW4oKTtcclxuICAgICAgICAgICAgLy8gSWYgbm8gdGFza3Mgd2VyZSByZWdpc3RlcmVkIHN5bmNocm9ub3VzbHksIHRoZW4gd2UncmUgZG9uZSBhbHJlYWR5XHJcbiAgICAgICAgICAgIGlmIChzdGF0ZS5udW1SZW1haW5pbmdUYXNrcyA9PT0gMCAmJiAhc3RhdGUuaGFzSXNzdWVkU3VjY2Vzc0NhbGxiYWNrKSB7XHJcbiAgICAgICAgICAgICAgICBzdGF0ZS5oYXNJc3N1ZWRTdWNjZXNzQ2FsbGJhY2sgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgc2V0VGltZW91dChmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgc3RhdGUuY29tcGxldGlvbkNhbGxiYWNrKC8qIGVycm9yICovIG51bGwpO1xyXG4gICAgICAgICAgICAgICAgfSwgMCk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICAgICAgY2F0Y2ggKGV4KSB7XHJcbiAgICAgICAgICAgIHN0YXRlLmNvbXBsZXRpb25DYWxsYmFjayhleCk7XHJcbiAgICAgICAgfVxyXG4gICAgfSk7XHJcbiAgICByZXR1cm4gc3luY2hyb25vdXNSZXN1bHQ7XHJcbn1cclxuZXhwb3J0cy5ydW4gPSBydW47XHJcbmZ1bmN0aW9uIGJhc2VVcmwodXJsKSB7XHJcbiAgICBpZiAodXJsKSB7XHJcbiAgICAgICAgaWYgKGRvbWFpbi5hY3RpdmUpIHtcclxuICAgICAgICAgICAgLy8gVGhlcmUncyBhbiBhY3RpdmUgZG9tYWluIChlLmcuLCBpbiBOb2RlLmpzKSwgc28gYXNzb2NpYXRlIHRoZSBiYXNlIFVSTCB3aXRoIGl0XHJcbiAgICAgICAgICAgIGRvbWFpbkNvbnRleHQuc2V0KGRvbWFpblRhc2tCYXNlVXJsU3RhdGVLZXksIHVybCk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGVsc2Uge1xyXG4gICAgICAgICAgICAvLyBUaGVyZSdzIG5vIGFjdGl2ZSBkb21haW4gKGUuZy4sIGluIGJyb3dzZXIpLCBzbyB0aGVyZSdzIGp1c3Qgb25lIHNoYXJlZCBiYXNlIFVSTFxyXG4gICAgICAgICAgICBub0RvbWFpbkJhc2VVcmwgPSB1cmw7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG4gICAgcmV0dXJuIGRvbWFpbi5hY3RpdmUgPyBkb21haW5Db250ZXh0LmdldChkb21haW5UYXNrQmFzZVVybFN0YXRlS2V5KSA6IG5vRG9tYWluQmFzZVVybDtcclxufVxyXG5leHBvcnRzLmJhc2VVcmwgPSBiYXNlVXJsO1xyXG5cblxuXG4vLy8vLy8vLy8vLy8vLy8vLy9cbi8vIFdFQlBBQ0sgRk9PVEVSXG4vLyAuL25vZGVfbW9kdWxlcy9kb21haW4tdGFzay9tYWluLmpzXG4vLyBtb2R1bGUgaWQgPSA1XG4vLyBtb2R1bGUgY2h1bmtzID0gMCIsIi8vIEdlbmVyYXRlZCBieSBDb2ZmZWVTY3JpcHQgMS42LjJcbnZhciBkb21haW47XG5cbmRvbWFpbiA9IHJlcXVpcmUoJ2RvbWFpbicpO1xuXG5leHBvcnRzLmNvbnRleHQgPSBmdW5jdGlvbihjb250ZXh0LCBjdXJyZW50RG9tYWluKSB7XG4gIGlmIChjdXJyZW50RG9tYWluID09IG51bGwpIHtcbiAgICBjdXJyZW50RG9tYWluID0gZG9tYWluLmFjdGl2ZTtcbiAgfVxuICBpZiAoY3VycmVudERvbWFpbiA9PSBudWxsKSB7XG4gICAgdGhyb3cgbmV3IEVycm9yKCdubyBhY3RpdmUgZG9tYWluJyk7XG4gIH1cbiAgcmV0dXJuIGN1cnJlbnREb21haW4uX19jb250ZXh0X18gPSBjb250ZXh0ICE9IG51bGwgPyBjb250ZXh0KCkgOiB7fTtcbn07XG5cbmV4cG9ydHMuY2xlYW51cCA9IGZ1bmN0aW9uKGNsZWFudXAsIGNvbnRleHQsIGN1cnJlbnREb21haW4pIHtcbiAgaWYgKGNvbnRleHQgPT0gbnVsbCkge1xuICAgIGNvbnRleHQgPSBudWxsO1xuICB9XG4gIGlmIChjdXJyZW50RG9tYWluID09IG51bGwpIHtcbiAgICBjdXJyZW50RG9tYWluID0gZG9tYWluLmFjdGl2ZTtcbiAgfVxuICBjb250ZXh0ID0gY29udGV4dCB8fCBjdXJyZW50RG9tYWluLl9fY29udGV4dF9fO1xuICBpZiAoKGNsZWFudXAgIT0gbnVsbCkgJiYgKGNvbnRleHQgIT0gbnVsbCkpIHtcbiAgICBjbGVhbnVwKGNvbnRleHQpO1xuICB9XG4gIGlmIChjdXJyZW50RG9tYWluICE9IG51bGwpIHtcbiAgICByZXR1cm4gY3VycmVudERvbWFpbi5fX2NvbnRleHRfXyA9IG51bGw7XG4gIH1cbn07XG5cbmV4cG9ydHMub25FcnJvciA9IGZ1bmN0aW9uKGVyciwgb25FcnJvciwgY29udGV4dCwgY3VycmVudERvbWFpbikge1xuICBpZiAoY29udGV4dCA9PSBudWxsKSB7XG4gICAgY29udGV4dCA9IG51bGw7XG4gIH1cbiAgaWYgKGN1cnJlbnREb21haW4gPT0gbnVsbCkge1xuICAgIGN1cnJlbnREb21haW4gPSBkb21haW4uYWN0aXZlO1xuICB9XG4gIGNvbnRleHQgPSBjb250ZXh0IHx8IGN1cnJlbnREb21haW4uX19jb250ZXh0X187XG4gIGlmIChvbkVycm9yICE9IG51bGwpIHtcbiAgICBvbkVycm9yKGVyciwgY29udGV4dCk7XG4gIH1cbiAgcmV0dXJuIGN1cnJlbnREb21haW4uX19jb250ZXh0X18gPSBudWxsO1xufTtcblxuZXhwb3J0cy5nZXQgPSBmdW5jdGlvbihrZXksIGN1cnJlbnREb21haW4pIHtcbiAgaWYgKGN1cnJlbnREb21haW4gPT0gbnVsbCkge1xuICAgIGN1cnJlbnREb21haW4gPSBkb21haW4uYWN0aXZlO1xuICB9XG4gIGlmIChjdXJyZW50RG9tYWluID09IG51bGwpIHtcbiAgICB0aHJvdyBuZXcgRXJyb3IoJ25vIGFjdGl2ZSBkb21haW4nKTtcbiAgfVxuICByZXR1cm4gY3VycmVudERvbWFpbi5fX2NvbnRleHRfX1trZXldO1xufTtcblxuZXhwb3J0cy5zZXQgPSBmdW5jdGlvbihrZXksIHZhbHVlLCBjdXJyZW50RG9tYWluKSB7XG4gIGlmIChjdXJyZW50RG9tYWluID09IG51bGwpIHtcbiAgICBjdXJyZW50RG9tYWluID0gZG9tYWluLmFjdGl2ZTtcbiAgfVxuICBpZiAoY3VycmVudERvbWFpbiA9PSBudWxsKSB7XG4gICAgdGhyb3cgbmV3IEVycm9yKCdubyBhY3RpdmUgZG9tYWluJyk7XG4gIH1cbiAgcmV0dXJuIGN1cnJlbnREb21haW4uX19jb250ZXh0X19ba2V5XSA9IHZhbHVlO1xufTtcblxuZXhwb3J0cy5ydW4gPSBmdW5jdGlvbihvcHRpb25zLCBmdW5jKSB7XG4gIHZhciBjbGVhbnVwLCBjb250ZXh0LCBjdXJyZW50RG9tYWluLCBlcnIsIG9uRXJyb3I7XG5cbiAgaWYgKCFmdW5jKSB7XG4gICAgZnVuYyA9IG9wdGlvbnM7XG4gICAgb3B0aW9ucyA9IHt9O1xuICB9XG4gIGNvbnRleHQgPSBvcHRpb25zLmNvbnRleHQsIGNsZWFudXAgPSBvcHRpb25zLmNsZWFudXAsIG9uRXJyb3IgPSBvcHRpb25zLm9uRXJyb3I7XG4gIGN1cnJlbnREb21haW4gPSBvcHRpb25zLmRvbWFpbiB8fCBkb21haW4uYWN0aXZlO1xuICBpZiAoIWN1cnJlbnREb21haW4pIHtcbiAgICB0aHJvdyBuZXcgRXJyb3IoJ25vIGFjdGl2ZSBkb21haW4nKTtcbiAgfVxuICBjdXJyZW50RG9tYWluLm9uKCdkaXNwb3NlJywgZnVuY3Rpb24oKSB7XG4gICAgcmV0dXJuIGV4cG9ydHMuY2xlYW51cChjbGVhbnVwLCBudWxsLCBjdXJyZW50RG9tYWluKTtcbiAgfSk7XG4gIGN1cnJlbnREb21haW4ub24oJ2Vycm9yJywgZnVuY3Rpb24oZXJyKSB7XG4gICAgaWYgKG9uRXJyb3IgIT0gbnVsbCkge1xuICAgICAgcmV0dXJuIGV4cG9ydHMub25FcnJvcihlcnIsIG9uRXJyb3IsIG51bGwsIGN1cnJlbnREb21haW4pO1xuICAgIH0gZWxzZSB7XG4gICAgICByZXR1cm4gZXhwb3J0cy5jbGVhbnVwKGNsZWFudXAsIG51bGwsIGN1cnJlbnREb21haW4pO1xuICAgIH1cbiAgfSk7XG4gIGV4cG9ydHMuY29udGV4dChjb250ZXh0LCBjdXJyZW50RG9tYWluKTtcbiAgdHJ5IHtcbiAgICBjdXJyZW50RG9tYWluLmJpbmQoZnVuYywgdHJ1ZSkoKTtcbiAgfSBjYXRjaCAoX2Vycm9yKSB7XG4gICAgZXJyID0gX2Vycm9yO1xuICAgIGN1cnJlbnREb21haW4uZW1pdCgnZXJyb3InLCBlcnIpO1xuICB9XG4gIHJldHVybiBjdXJyZW50RG9tYWluO1xufTtcblxuZXhwb3J0cy5ydW5Jbk5ld0RvbWFpbiA9IGZ1bmN0aW9uKG9wdGlvbnMsIGZ1bmMpIHtcbiAgdmFyIGN1cnJlbnREb21haW47XG5cbiAgaWYgKCFmdW5jKSB7XG4gICAgZnVuYyA9IG9wdGlvbnM7XG4gICAgb3B0aW9ucyA9IHt9O1xuICB9XG4gIGN1cnJlbnREb21haW4gPSBkb21haW4uYWN0aXZlO1xuICBvcHRpb25zLmRvbWFpbiA9IGRvbWFpbi5jcmVhdGUoKTtcbiAgaWYgKCFvcHRpb25zLmRldGFjaCAmJiBjdXJyZW50RG9tYWluKSB7XG4gICAgY3VycmVudERvbWFpbi5hZGQob3B0aW9ucy5kb21haW4pO1xuICAgIG9wdGlvbnMuZG9tYWluLm9uKCdlcnJvcicsIGZ1bmN0aW9uKGVycikge1xuICAgICAgcmV0dXJuIGN1cnJlbnREb21haW4uZW1pdCgnZXJyb3InLCBlcnIpO1xuICAgIH0pO1xuICAgIGN1cnJlbnREb21haW4ub24oJ2Rpc3Bvc2UnLCBmdW5jdGlvbigpIHtcbiAgICAgIHJldHVybiBvcHRpb25zLmRvbWFpbi5kaXNwb3NlKCk7XG4gICAgfSk7XG4gIH1cbiAgcmV0dXJuIGV4cG9ydHMucnVuKG9wdGlvbnMsIGZ1bmMpO1xufTtcblxuZXhwb3J0cy5taWRkbGV3YXJlID0gZnVuY3Rpb24oY29udGV4dCwgY2xlYW51cCkge1xuICByZXR1cm4gZnVuY3Rpb24ocmVxLCByZXMsIG5leHQpIHtcbiAgICB2YXIgY3VycmVudERvbWFpbiwgX3JlZjtcblxuICAgIGlmICh0eXBlb2YgY29udGV4dCAhPT0gJ2Z1bmN0aW9uJykge1xuICAgICAgX3JlZiA9IGNvbnRleHQsIGNvbnRleHQgPSBfcmVmLmNvbnRleHQsIGNsZWFudXAgPSBfcmVmLmNsZWFudXA7XG4gICAgfVxuICAgIGN1cnJlbnREb21haW4gPSBkb21haW4uYWN0aXZlO1xuICAgIGV4cG9ydHMuY29udGV4dChjb250ZXh0LCBjdXJyZW50RG9tYWluKTtcbiAgICByZXMub24oJ2ZpbmlzaCcsIGZ1bmN0aW9uKCkge1xuICAgICAgcmV0dXJuIGV4cG9ydHMuY2xlYW51cChjbGVhbnVwLCBudWxsLCBjdXJyZW50RG9tYWluKTtcbiAgICB9KTtcbiAgICByZXEuX19jb250ZXh0X18gPSBjdXJyZW50RG9tYWluLl9fY29udGV4dF9fO1xuICAgIHJldHVybiBuZXh0KCk7XG4gIH07XG59O1xuXG5leHBvcnRzLm1pZGRsZXdhcmVPbkVycm9yID0gZnVuY3Rpb24ob25FcnJvcikge1xuICByZXR1cm4gZnVuY3Rpb24oZXJyLCByZXEsIHJlcywgbmV4dCkge1xuICAgIGlmICh0eXBlb2Ygb25FcnJvciAhPT0gJ2Z1bmN0aW9uJykge1xuICAgICAgb25FcnJvciA9IG9uRXJyb3Iub25FcnJvcjtcbiAgICB9XG4gICAgaWYgKG9uRXJyb3IgIT0gbnVsbCkge1xuICAgICAgZXhwb3J0cy5vbkVycm9yKGVyciwgb25FcnJvciwgcmVxLl9fY29udGV4dF9fKTtcbiAgICB9IGVsc2Uge1xuICAgICAgZXhwb3J0cy5jbGVhbnVwKG9uRXJyb3IsIHJlcS5fX2NvbnRleHRfXyk7XG4gICAgfVxuICAgIHJlcS5fX2NvbnRleHRfXyA9IG51bGw7XG4gICAgcmV0dXJuIG5leHQoZXJyKTtcbiAgfTtcbn07XG5cblxuXG4vLy8vLy8vLy8vLy8vLy8vLy9cbi8vIFdFQlBBQ0sgRk9PVEVSXG4vLyAuL25vZGVfbW9kdWxlcy9kb21haW4tY29udGV4dC9saWIvaW5kZXguanNcbi8vIG1vZHVsZSBpZCA9IDZcbi8vIG1vZHVsZSBjaHVua3MgPSAwIl0sInNvdXJjZVJvb3QiOiIifQ==