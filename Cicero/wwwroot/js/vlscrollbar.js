(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
  typeof define === 'function' && define.amd ? define(factory) :
  (global = global || self, global.VesuvioBar = factory());
}(this, function () { 'use strict';

  var aFunction = function (it) {
    if (typeof it != 'function') {
      throw TypeError(String(it) + ' is not a function');
    } return it;
  };
  var bindContext = function (fn, that, length) {
    aFunction(fn);
    if (that === undefined) return fn;
    switch (length) {
      case 0: return function () {
        return fn.call(that);
      };
      case 1: return function (a) {
        return fn.call(that, a);
      };
      case 2: return function (a, b) {
        return fn.call(that, a, b);
      };
      case 3: return function (a, b, c) {
        return fn.call(that, a, b, c);
      };
    }
    return function (/* ...args */) {
      return fn.apply(that, arguments);
    };
  };

  var fails = function (exec) {
    try {
      return !!exec();
    } catch (error) {
      return true;
    }
  };

  var toString = {}.toString;

  var classofRaw = function (it) {
    return toString.call(it).slice(8, -1);
  };


  var split = ''.split;

  var indexedObject = fails(function () {
    return !Object('z').propertyIsEnumerable(0);
  }) ? function (it) {
    return classofRaw(it) == 'String' ? split.call(it, '') : Object(it);
  } : Object;

  var requireObjectCoercible = function (it) {
    if (it == undefined) throw TypeError("Can't call method on " + it);
    return it;
  };

  var toObject = function (argument) {
    return Object(requireObjectCoercible(argument));
  };

  var ceil = Math.ceil;
  var floor = Math.floor;
 
  var toInteger = function (argument) {
    return isNaN(argument = +argument) ? 0 : (argument > 0 ? floor : ceil)(argument);
  };

  var min = Math.min;

  var toLength = function (argument) {
    return argument > 0 ? min(toInteger(argument), 0x1FFFFFFFFFFFFF) : 0; // 2 ** 53 - 1 == 9007199254740991
  };

  var isObject = function (it) {
    return typeof it === 'object' ? it !== null : typeof it === 'function';
  };
  
  var isArray = Array.isArray || function isArray(arg) {
    return classofRaw(arg) == 'Array';
  };

  var commonjsGlobal = typeof globalThis !== 'undefined' ? globalThis : typeof window !== 'undefined' ? window : typeof global !== 'undefined' ? global : typeof self !== 'undefined' ? self : {};

  function createCommonjsModule(fn, module) {
    return module = { exports: {} }, fn(module, module.exports), module.exports;
  }
  var global$1 = typeof window == 'object' && window && window.Math == Math ? window
    : typeof self == 'object' && self && self.Math == Math ? self
    : Function('return this')();
  var descriptors = !fails(function () {
    return Object.defineProperty({}, 'a', { get: function () { return 7; } }).a != 7;
  });

  var document$1 = global$1.document;
  var exist = isObject(document$1) && isObject(document$1.createElement);

  var documentCreateElement = function (it) {
    return exist ? document$1.createElement(it) : {};
  };
  var ie8DomDefine = !descriptors && !fails(function () {
    return Object.defineProperty(documentCreateElement('div'), 'a', {
      get: function () { return 7; }
    }).a != 7;
  });

  var anObject = function (it) {
    if (!isObject(it)) {
      throw TypeError(String(it) + ' is not an object');
    } return it;
  };


  var toPrimitive = function (it, S) {
    if (!isObject(it)) return it;
    var fn, val;
    if (S && typeof (fn = it.toString) == 'function' && !isObject(val = fn.call(it))) return val;
    if (typeof (fn = it.valueOf) == 'function' && !isObject(val = fn.call(it))) return val;
    if (!S && typeof (fn = it.toString) == 'function' && !isObject(val = fn.call(it))) return val;
    throw TypeError("Can't convert object to primitive value");
  };

  var nativeDefineProperty = Object.defineProperty;

  var f = descriptors ? nativeDefineProperty : function defineProperty(O, P, Attributes) {
    anObject(O);
    P = toPrimitive(P, true);
    anObject(Attributes);
    if (ie8DomDefine) try {
      return nativeDefineProperty(O, P, Attributes);
    } catch (error) { /* empty */ }
    if ('get' in Attributes || 'set' in Attributes) throw TypeError('Accessors not supported');
    if ('value' in Attributes) O[P] = Attributes.value;
    return O;
  };

  var objectDefineProperty = {
    f: f
  };

  var createPropertyDescriptor = function (bitmap, value) {
    return {
      enumerable: !(bitmap & 1),
      configurable: !(bitmap & 2),
      writable: !(bitmap & 4),
      value: value
    };
  };

  var hide = descriptors ? function (object, key, value) {
    return objectDefineProperty.f(object, key, createPropertyDescriptor(1, value));
  } : function (object, key, value) {
    object[key] = value;
    return object;
  };

  var setGlobal = function (key, value) {
    try {
      hide(global$1, key, value);
    } catch (error) {
      global$1[key] = value;
    } return value;
  };

  var shared = createCommonjsModule(function (module) {
  var SHARED = '__core-js_shared__';
  var store = global$1[SHARED] || setGlobal(SHARED, {});

  (module.exports = function (key, value) {
    return store[key] || (store[key] = value !== undefined ? value : {});
  })('versions', []).push({
    version: '1',
    mode: 'global',
    copyright: '© 2019 Vesuvio Labs'
  });
  });

  var id = 0;
  var postfix = Math.random();

  var uid = function (key) {
    return 'Symbol('.concat(key === undefined ? '' : key, ')_', (++id + postfix).toString(36));
  };
  var nativeSymbol = !fails(function () {
    return !String(Symbol());
  });

  var store = shared('wks');

  var Symbol$1 = global$1.Symbol;


  var wellKnownSymbol = function (name) {
    return store[name] || (store[name] = nativeSymbol && Symbol$1[name]
      || (nativeSymbol ? Symbol$1 : uid)('Symbol.' + name));
  };

  var SPECIES = wellKnownSymbol('species');

  var arraySpeciesCreate = function (originalArray, length) {
    var C;
    if (isArray(originalArray)) {
      C = originalArray.constructor;
      if (typeof C == 'function' && (C === Array || isArray(C.prototype))) C = undefined;
      else if (isObject(C)) {
        C = C[SPECIES];
        if (C === null) C = undefined;
      }
    } return new (C === undefined ? Array : C)(length === 0 ? 0 : length);
  };
  
  var arrayMethods = function (TYPE, specificCreate) {
    var IS_MAP = TYPE == 1;
    var IS_FILTER = TYPE == 2;
    var IS_SOME = TYPE == 3;
    var IS_EVERY = TYPE == 4;
    var IS_FIND_INDEX = TYPE == 6;
    var NO_HOLES = TYPE == 5 || IS_FIND_INDEX;
    var create = specificCreate || arraySpeciesCreate;
    return function ($this, callbackfn, that) {
      var O = toObject($this);
      var self = indexedObject(O);
      var boundFunction = bindContext(callbackfn, that, 3);
      var length = toLength(self.length);
      var index = 0;
      var target = IS_MAP ? create($this, length) : IS_FILTER ? create($this, 0) : undefined;
      var value, result;
      for (;length > index; index++) if (NO_HOLES || index in self) {
        value = self[index];
        result = boundFunction(value, index, O);
        if (TYPE) {
          if (IS_MAP) target[index] = result; // map
          else if (result) switch (TYPE) {
            case 3: return true;              // some
            case 5: return value;             // find
            case 6: return index;             // findIndex
            case 2: target.push(value);       // filter
          } else if (IS_EVERY) return false;  // every
        }
      }
      return IS_FIND_INDEX ? -1 : IS_SOME || IS_EVERY ? IS_EVERY : target;
    };
  };

  var SPECIES$1 = wellKnownSymbol('species');

  var arrayMethodHasSpeciesSupport = function (METHOD_NAME) {
    return !fails(function () {
      var array = [];
      var constructor = array.constructor = {};
      constructor[SPECIES$1] = function () {
        return { foo: 1 };
      };
      return array[METHOD_NAME](Boolean).foo !== 1;
    });
  };

  var nativePropertyIsEnumerable = {}.propertyIsEnumerable;
  var nativeGetOwnPropertyDescriptor = Object.getOwnPropertyDescriptor;
  var NASHORN_BUG = nativeGetOwnPropertyDescriptor && !nativePropertyIsEnumerable.call({ 1: 2 }, 1);

  var f$1 = NASHORN_BUG ? function propertyIsEnumerable(V) {
    var descriptor = nativeGetOwnPropertyDescriptor(this, V);
    return !!descriptor && descriptor.enumerable;
  } : nativePropertyIsEnumerable;

  var objectPropertyIsEnumerable = {
    f: f$1
  };



  var toIndexedObject = function (it) {
    return indexedObject(requireObjectCoercible(it));
  };

  var hasOwnProperty = {}.hasOwnProperty;

  var has = function (it, key) {
    return hasOwnProperty.call(it, key);
  };

  var nativeGetOwnPropertyDescriptor$1 = Object.getOwnPropertyDescriptor;

  var f$2 = descriptors ? nativeGetOwnPropertyDescriptor$1 : function getOwnPropertyDescriptor(O, P) {
    O = toIndexedObject(O);
    P = toPrimitive(P, true);
    if (ie8DomDefine) try {
      return nativeGetOwnPropertyDescriptor$1(O, P);
    } catch (error) { /* empty */ }
    if (has(O, P)) return createPropertyDescriptor(!objectPropertyIsEnumerable.f.call(O, P), O[P]);
  };

  var objectGetOwnPropertyDescriptor = {
    f: f$2
  };

  var functionToString = shared('native-function-to-string', Function.toString);

  var WeakMap$1 = global$1.WeakMap;

  var nativeWeakMap = typeof WeakMap$1 === 'function' && /native code/.test(functionToString.call(WeakMap$1));

  var shared$1 = shared('keys');


  var sharedKey = function (key) {
    return shared$1[key] || (shared$1[key] = uid(key));
  };

  var hiddenKeys = {};

  var WeakMap$2 = global$1.WeakMap;
  var set, get, has$1;

  var enforce = function (it) {
    return has$1(it) ? get(it) : set(it, {});
  };

  var getterFor = function (TYPE) {
    return function (it) {
      var state;
      if (!isObject(it) || (state = get(it)).type !== TYPE) {
        throw TypeError('Incompatible receiver, ' + TYPE + ' required');
      } return state;
    };
  };

  if (nativeWeakMap) {
    var store$1 = new WeakMap$2();
    var wmget = store$1.get;
    var wmhas = store$1.has;
    var wmset = store$1.set;
    set = function (it, metadata) {
      wmset.call(store$1, it, metadata);
      return metadata;
    };
    get = function (it) {
      return wmget.call(store$1, it) || {};
    };
    has$1 = function (it) {
      return wmhas.call(store$1, it);
    };
  } else {
    var STATE = sharedKey('state');
    hiddenKeys[STATE] = true;
    set = function (it, metadata) {
      hide(it, STATE, metadata);
      return metadata;
    };
    get = function (it) {
      return has(it, STATE) ? it[STATE] : {};
    };
    has$1 = function (it) {
      return has(it, STATE);
    };
  }

  var internalState = {
    set: set,
    get: get,
    has: has$1,
    enforce: enforce,
    getterFor: getterFor
  };

  var redefine = createCommonjsModule(function (module) {
  var getInternalState = internalState.get;
  var enforceInternalState = internalState.enforce;
  var TEMPLATE = String(functionToString).split('toString');

  shared('inspectSource', function (it) {
    return functionToString.call(it);
  });

  (module.exports = function (O, key, value, options) {
    var unsafe = options ? !!options.unsafe : false;
    var simple = options ? !!options.enumerable : false;
    var noTargetGet = options ? !!options.noTargetGet : false;
    if (typeof value == 'function') {
      if (typeof key == 'string' && !has(value, 'name')) hide(value, 'name', key);
      enforceInternalState(value).source = TEMPLATE.join(typeof key == 'string' ? key : '');
    }
    if (O === global$1) {
      if (simple) O[key] = value;
      else setGlobal(key, value);
      return;
    } else if (!unsafe) {
      delete O[key];
    } else if (!noTargetGet && O[key]) {
      simple = true;
    }
    if (simple) O[key] = value;
    else hide(O, key, value);
  })(Function.prototype, 'toString', function toString() {
    return typeof this == 'function' && getInternalState(this).source || functionToString.call(this);
  });
  });

  var max = Math.max;
  var min$1 = Math.min;
  // Let integer be ? ToInteger(index).
  var toAbsoluteIndex = function (index, length) {
    var integer = toInteger(index);
    return integer < 0 ? max(integer + length, 0) : min$1(integer, length);
  };
  // false -> Array#indexOf
  // true  -> Array#includes
  var arrayIncludes = function (IS_INCLUDES) {
    return function ($this, el, fromIndex) {
      var O = toIndexedObject($this);
      var length = toLength(O.length);
      var index = toAbsoluteIndex(fromIndex, length);
      var value;
      // eslint-disable-next-line no-self-compare
      if (IS_INCLUDES && el != el) while (length > index) {
        value = O[index++];
        if (value != value) return true;
      } else for (;length > index; index++) if (IS_INCLUDES || index in O) {
        if (O[index] === el) return IS_INCLUDES || index || 0;
      } return !IS_INCLUDES && -1;
    };
  };

  var arrayIndexOf = arrayIncludes(false);


  var objectKeysInternal = function (object, names) {
    var O = toIndexedObject(object);
    var i = 0;
    var result = [];
    var key;
    for (key in O) !has(hiddenKeys, key) && has(O, key) && result.push(key);
    while (names.length > i) if (has(O, key = names[i++])) {
      ~arrayIndexOf(result, key) || result.push(key);
    }
    return result;
  };
  var enumBugKeys = [
    'constructor',
    'hasOwnProperty',
    'isPrototypeOf',
    'propertyIsEnumerable',
    'toLocaleString',
    'toString',
    'valueOf'
  ];

  var hiddenKeys$1 = enumBugKeys.concat('length', 'prototype');

  var f$3 = Object.getOwnPropertyNames || function getOwnPropertyNames(O) {
    return objectKeysInternal(O, hiddenKeys$1);
  };

  var objectGetOwnPropertyNames = {
    f: f$3
  };

  var f$4 = Object.getOwnPropertySymbols;

  var objectGetOwnPropertySymbols = {
    f: f$4
  };

  var Reflect = global$1.Reflect;
  var ownKeys = Reflect && Reflect.ownKeys || function ownKeys(it) {
    var keys = objectGetOwnPropertyNames.f(anObject(it));
    var getOwnPropertySymbols = objectGetOwnPropertySymbols.f;
    return getOwnPropertySymbols ? keys.concat(getOwnPropertySymbols(it)) : keys;
  };

  var copyConstructorProperties = function (target, source) {
    var keys = ownKeys(source);
    var defineProperty = objectDefineProperty.f;
    var getOwnPropertyDescriptor = objectGetOwnPropertyDescriptor.f;
    for (var i = 0; i < keys.length; i++) {
      var key = keys[i];
      if (!has(target, key)) defineProperty(target, key, getOwnPropertyDescriptor(source, key));
    }
  };

  var replacement = /#|\.prototype\./;

  var isForced = function (feature, detection) {
    var value = data[normalize(feature)];
    return value == POLYFILL ? true
      : value == NATIVE ? false
      : typeof detection == 'function' ? fails(detection)
      : !!detection;
  };

  var normalize = isForced.normalize = function (string) {
    return String(string).replace(replacement, '.').toLowerCase();
  };

  var data = isForced.data = {};
  var NATIVE = isForced.NATIVE = 'N';
  var POLYFILL = isForced.POLYFILL = 'P';

  var isForced_1 = isForced;

  var getOwnPropertyDescriptor = objectGetOwnPropertyDescriptor.f;
  var _export = function (options, source) {
    var TARGET = options.target;
    var GLOBAL = options.global;
    var STATIC = options.stat;
    var FORCED, target, key, targetProperty, sourceProperty, descriptor;
    if (GLOBAL) {
      target = global$1;
    } else if (STATIC) {
      target = global$1[TARGET] || setGlobal(TARGET, {});
    } else {
      target = (global$1[TARGET] || {}).prototype;
    }
    if (target) for (key in source) {
      sourceProperty = source[key];
      if (options.noTargetGet) {
        descriptor = getOwnPropertyDescriptor(target, key);
        targetProperty = descriptor && descriptor.value;
      } else targetProperty = target[key];
      FORCED = isForced_1(GLOBAL ? key : TARGET + (STATIC ? '.' : '#') + key, options.forced);
      if (!FORCED && targetProperty !== undefined) {
        if (typeof sourceProperty === typeof targetProperty) continue;
        copyConstructorProperties(sourceProperty, targetProperty);
      }
      if (options.sham || (targetProperty && targetProperty.sham)) {
        hide(sourceProperty, 'sham', true);
      }
      redefine(target, key, sourceProperty, options);
    }
  };

  var internalFilter = arrayMethods(2);

  var SPECIES_SUPPORT = arrayMethodHasSpeciesSupport('filter');
  _export({ target: 'Array', proto: true, forced: !SPECIES_SUPPORT }, {
    filter: function filter(callbackfn /* , thisArg */) {
      return internalFilter(this, callbackfn, arguments[1]);
    }
  });

  var sloppyArrayMethod = function (METHOD_NAME, argument) {
    var method = [][METHOD_NAME];
    return !method || !fails(function () {
      method.call(null, argument || function () { throw 1; }, 1);
    });
  };

  var nativeForEach = [].forEach;
  var internalForEach = arrayMethods(0);

  var SLOPPY_METHOD = sloppyArrayMethod('forEach');
  var arrayForEach = SLOPPY_METHOD ? function forEach(callbackfn /* , thisArg */) {
    return internalForEach(this, callbackfn, arguments[1]);
  } : nativeForEach;
  _export({ target: 'Array', proto: true, forced: [].forEach != arrayForEach }, { forEach: arrayForEach });



  var objectKeys = Object.keys || function keys(O) {
    return objectKeysInternal(O, enumBugKeys);
  };

  var objectDefineProperties = descriptors ? Object.defineProperties : function defineProperties(O, Properties) {
    anObject(O);
    var keys = objectKeys(Properties);
    var length = keys.length;
    var i = 0;
    var key;
    while (length > i) objectDefineProperty.f(O, key = keys[i++], Properties[key]);
    return O;
  };

  var document$2 = global$1.document;

  var html = document$2 && document$2.documentElement;





  var IE_PROTO = sharedKey('IE_PROTO');
  var PROTOTYPE = 'prototype';
  var Empty = function () { /* empty */ };
  var createDict = function () {
    var iframe = documentCreateElement('iframe');
    var length = enumBugKeys.length;
    var lt = '<';
    var script = 'script';
    var gt = '>';
    var js = 'java' + script + ':';
    var iframeDocument;
    iframe.style.display = 'none';
    html.appendChild(iframe);
    iframe.src = String(js);
    iframeDocument = iframe.contentWindow.document;
    iframeDocument.open();
    iframeDocument.write(lt + script + gt + 'document.F=Object' + lt + '/' + script + gt);
    iframeDocument.close();
    createDict = iframeDocument.F;
    while (length--) delete createDict[PROTOTYPE][enumBugKeys[length]];
    return createDict();
  };

  var objectCreate = Object.create || function create(O, Properties) {
    var result;
    if (O !== null) {
      Empty[PROTOTYPE] = anObject(O);
      result = new Empty();
      Empty[PROTOTYPE] = null;
      result[IE_PROTO] = O;
    } else result = createDict();
    return Properties === undefined ? result : objectDefineProperties(result, Properties);
  };

  hiddenKeys[IE_PROTO] = true;

  var UNSCOPABLES = wellKnownSymbol('unscopables');


  var ArrayPrototype = Array.prototype;
  if (ArrayPrototype[UNSCOPABLES] == undefined) {
    hide(ArrayPrototype, UNSCOPABLES, objectCreate(null));
  }
  var addToUnscopables = function (key) {
    ArrayPrototype[UNSCOPABLES][key] = true;
  };

  var iterators = {};

  var correctPrototypeGetter = !fails(function () {
    function F() { /* empty */ }
    F.prototype.constructor = null;
    return Object.getPrototypeOf(new F()) !== F.prototype;
  });


  var IE_PROTO$1 = sharedKey('IE_PROTO');

  var ObjectPrototype = Object.prototype;

  var objectGetPrototypeOf = correctPrototypeGetter ? Object.getPrototypeOf : function (O) {
    O = toObject(O);
    if (has(O, IE_PROTO$1)) return O[IE_PROTO$1];
    if (typeof O.constructor == 'function' && O instanceof O.constructor) {
      return O.constructor.prototype;
    } return O instanceof Object ? ObjectPrototype : null;
  };

  var ITERATOR = wellKnownSymbol('iterator');
  var BUGGY_SAFARI_ITERATORS = false;

  var returnThis = function () { return this; };
  var IteratorPrototype, PrototypeOfArrayIteratorPrototype, arrayIterator;

  if ([].keys) {
    arrayIterator = [].keys();
    if (!('next' in arrayIterator)) BUGGY_SAFARI_ITERATORS = true;
    else {
      PrototypeOfArrayIteratorPrototype = objectGetPrototypeOf(objectGetPrototypeOf(arrayIterator));
      if (PrototypeOfArrayIteratorPrototype !== Object.prototype) IteratorPrototype = PrototypeOfArrayIteratorPrototype;
    }
  }

  if (IteratorPrototype == undefined) IteratorPrototype = {};
  if (!has(IteratorPrototype, ITERATOR)) hide(IteratorPrototype, ITERATOR, returnThis);

  var iteratorsCore = {
    IteratorPrototype: IteratorPrototype,
    BUGGY_SAFARI_ITERATORS: BUGGY_SAFARI_ITERATORS
  };

  var defineProperty = objectDefineProperty.f;

  var TO_STRING_TAG = wellKnownSymbol('toStringTag');

  var setToStringTag = function (it, TAG, STATIC) {
    if (it && !has(it = STATIC ? it : it.prototype, TO_STRING_TAG)) {
      defineProperty(it, TO_STRING_TAG, { configurable: true, value: TAG });
    }
  };

  var IteratorPrototype$1 = iteratorsCore.IteratorPrototype;





  var returnThis$1 = function () { return this; };

  var createIteratorConstructor = function (IteratorConstructor, NAME, next) {
    var TO_STRING_TAG = NAME + ' Iterator';
    IteratorConstructor.prototype = objectCreate(IteratorPrototype$1, { next: createPropertyDescriptor(1, next) });
    setToStringTag(IteratorConstructor, TO_STRING_TAG, false, true);
    iterators[TO_STRING_TAG] = returnThis$1;
    return IteratorConstructor;
  };

  var validateSetPrototypeOfArguments = function (O, proto) {
    anObject(O);
    if (!isObject(proto) && proto !== null) {
      throw TypeError("Can't set " + String(proto) + ' as a prototype');
    }
  };
  var objectSetPrototypeOf = Object.setPrototypeOf || ('__proto__' in {} ? function () {
    var correctSetter = false;
    var test = {};
    var setter;
    try {
      setter = Object.getOwnPropertyDescriptor(Object.prototype, '__proto__').set;
      setter.call(test, []);
      correctSetter = test instanceof Array;
    } catch (error) { /* empty */ }
    return function setPrototypeOf(O, proto) {
      validateSetPrototypeOfArguments(O, proto);
      if (correctSetter) setter.call(O, proto);
      else O.__proto__ = proto;
      return O;
    };
  }() : undefined);

  var ITERATOR$1 = wellKnownSymbol('iterator');


  var IteratorPrototype$2 = iteratorsCore.IteratorPrototype;
  var BUGGY_SAFARI_ITERATORS$1 = iteratorsCore.BUGGY_SAFARI_ITERATORS;
  var KEYS = 'keys';
  var VALUES = 'values';
  var ENTRIES = 'entries';

  var returnThis$2 = function () { return this; };

  var defineIterator = function (Iterable, NAME, IteratorConstructor, next, DEFAULT, IS_SET, FORCED) {
    createIteratorConstructor(IteratorConstructor, NAME, next);

    var getIterationMethod = function (KIND) {
      if (KIND === DEFAULT && defaultIterator) return defaultIterator;
      if (!BUGGY_SAFARI_ITERATORS$1 && KIND in IterablePrototype) return IterablePrototype[KIND];
      switch (KIND) {
        case KEYS: return function keys() { return new IteratorConstructor(this, KIND); };
        case VALUES: return function values() { return new IteratorConstructor(this, KIND); };
        case ENTRIES: return function entries() { return new IteratorConstructor(this, KIND); };
      } return function () { return new IteratorConstructor(this); };
    };

    var TO_STRING_TAG = NAME + ' Iterator';
    var INCORRECT_VALUES_NAME = false;
    var IterablePrototype = Iterable.prototype;
    var nativeIterator = IterablePrototype[ITERATOR$1]
      || IterablePrototype['@@iterator']
      || DEFAULT && IterablePrototype[DEFAULT];
    var defaultIterator = !BUGGY_SAFARI_ITERATORS$1 && nativeIterator || getIterationMethod(DEFAULT);
    var anyNativeIterator = NAME == 'Array' ? IterablePrototype.entries || nativeIterator : nativeIterator;
    var CurrentIteratorPrototype, methods, KEY;
    if (anyNativeIterator) {
      CurrentIteratorPrototype = objectGetPrototypeOf(anyNativeIterator.call(new Iterable()));
      if (IteratorPrototype$2 !== Object.prototype && CurrentIteratorPrototype.next) {
        if (objectGetPrototypeOf(CurrentIteratorPrototype) !== IteratorPrototype$2) {
          if (objectSetPrototypeOf) {
            objectSetPrototypeOf(CurrentIteratorPrototype, IteratorPrototype$2);
          } else if (typeof CurrentIteratorPrototype[ITERATOR$1] != 'function') {
            hide(CurrentIteratorPrototype, ITERATOR$1, returnThis$2);
          }
        }
        setToStringTag(CurrentIteratorPrototype, TO_STRING_TAG, true, true);
      }
    }
    if (DEFAULT == VALUES && nativeIterator && nativeIterator.name !== VALUES) {
      INCORRECT_VALUES_NAME = true;
      defaultIterator = function values() { return nativeIterator.call(this); };
    }
    if (IterablePrototype[ITERATOR$1] !== defaultIterator) {
      hide(IterablePrototype, ITERATOR$1, defaultIterator);
    }
    iterators[NAME] = defaultIterator;
    if (DEFAULT) {
      methods = {
        values: getIterationMethod(VALUES),
        keys: IS_SET ? defaultIterator : getIterationMethod(KEYS),
        entries: getIterationMethod(ENTRIES)
      };
      if (FORCED) for (KEY in methods) {
        if (BUGGY_SAFARI_ITERATORS$1 || INCORRECT_VALUES_NAME || !(KEY in IterablePrototype)) {
          redefine(IterablePrototype, KEY, methods[KEY]);
        }
      } else _export({ target: NAME, proto: true, forced: BUGGY_SAFARI_ITERATORS$1 || INCORRECT_VALUES_NAME }, methods);
    }

    return methods;
  };

  var ARRAY_ITERATOR = 'Array Iterator';
  var setInternalState = internalState.set;
  var getInternalState = internalState.getterFor(ARRAY_ITERATOR);

  var es_array_iterator = defineIterator(Array, 'Array', function (iterated, kind) {
    setInternalState(this, {
      type: ARRAY_ITERATOR,
      target: toIndexedObject(iterated), // target
      index: 0,                          // next index
      kind: kind                         // kind
    });

  }, function () {
    var state = getInternalState(this);
    var target = state.target;
    var kind = state.kind;
    var index = state.index++;
    if (!target || index >= target.length) {
      state.target = undefined;
      return { value: undefined, done: true };
    }
    if (kind == 'keys') return { value: index, done: false };
    if (kind == 'values') return { value: target[index], done: false };
    return { value: [index, target[index]], done: false };
  }, 'values');

  iterators.Arguments = iterators.Array;
  addToUnscopables('keys');
  addToUnscopables('values');
  addToUnscopables('entries');
 
  var arrayReduce = function (that, callbackfn, argumentsLength, memo, isRight) {
    aFunction(callbackfn);
    var O = toObject(that);
    var self = indexedObject(O);
    var length = toLength(O.length);
    var index = isRight ? length - 1 : 0;
    var i = isRight ? -1 : 1;
    if (argumentsLength < 2) while (true) {
      if (index in self) {
        memo = self[index];
        index += i;
        break;
      }
      index += i;
      if (isRight ? index < 0 : length <= index) {
        throw TypeError('Reduce of empty array with no initial value');
      }
    }
    for (;isRight ? index >= 0 : length > index; index += i) if (index in self) {
      memo = callbackfn(memo, self[index], index, O);
    }
    return memo;
  };

  var SLOPPY_METHOD$1 = sloppyArrayMethod('reduce');

  _export({ target: 'Array', proto: true, forced: SLOPPY_METHOD$1 }, {
    reduce: function reduce(callbackfn /* , initialValue */) {
      return arrayReduce(this, callbackfn, arguments.length, arguments[1], false);
    }
  });

  var defineProperty$1 = objectDefineProperty.f;
  var FunctionPrototype = Function.prototype;
  var FunctionPrototypeToString = FunctionPrototype.toString;
  var nameRE = /^\s*function ([^ (]*)/;
  var NAME = 'name';

  if (descriptors && !(NAME in FunctionPrototype)) {
    defineProperty$1(FunctionPrototype, NAME, {
      configurable: true,
      get: function () {
        try {
          return FunctionPrototypeToString.call(this).match(nameRE)[1];
        } catch (error) {
          return '';
        }
      }
    });
  }





  var nativeAssign = Object.assign;
  var objectAssign = !nativeAssign || fails(function () {
    var A = {};
    var B = {};
    var symbol = Symbol();
    var alphabet = 'abcdefghijklmnopqrst';
    A[symbol] = 7;
    alphabet.split('').forEach(function (chr) { B[chr] = chr; });
    return nativeAssign({}, A)[symbol] != 7 || objectKeys(nativeAssign({}, B)).join('') != alphabet;
  }) ? function assign(target, source) { // eslint-disable-line no-unused-vars
    var T = toObject(target);
    var argumentsLength = arguments.length;
    var index = 1;
    var getOwnPropertySymbols = objectGetOwnPropertySymbols.f;
    var propertyIsEnumerable = objectPropertyIsEnumerable.f;
    while (argumentsLength > index) {
      var S = indexedObject(arguments[index++]);
      var keys = getOwnPropertySymbols ? objectKeys(S).concat(getOwnPropertySymbols(S)) : objectKeys(S);
      var length = keys.length;
      var j = 0;
      var key;
      while (length > j) if (propertyIsEnumerable.call(S, key = keys[j++])) T[key] = S[key];
    } return T;
  } : nativeAssign;

  _export({ target: 'Object', stat: true, forced: Object.assign !== objectAssign }, { assign: objectAssign });

  var TO_STRING_TAG$1 = wellKnownSymbol('toStringTag');
  var CORRECT_ARGUMENTS = classofRaw(function () { return arguments; }()) == 'Arguments';
  var tryGet = function (it, key) {
    try {
      return it[key];
    } catch (error) { /* empty */ }
  };
  var classof = function (it) {
    var O, tag, result;
    return it === undefined ? 'Undefined' : it === null ? 'Null'
      : typeof (tag = tryGet(O = Object(it), TO_STRING_TAG$1)) == 'string' ? tag
      : CORRECT_ARGUMENTS ? classofRaw(O)
      : (result = classofRaw(O)) == 'Object' && typeof O.callee == 'function' ? 'Arguments' : result;
  };

  var TO_STRING_TAG$2 = wellKnownSymbol('toStringTag');
  var test = {};

  test[TO_STRING_TAG$2] = 'z';

  var objectToString = String(test) !== '[object z]' ? function toString() {
    return '[object ' + classof(this) + ']';
  } : test.toString;

  var ObjectPrototype$1 = Object.prototype;

  if (objectToString !== ObjectPrototype$1.toString) {
    redefine(ObjectPrototype$1, 'toString', objectToString, { unsafe: true });
  }

  var whitespaces = '\u0009\u000A\u000B\u000C\u000D\u0020\u00A0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000\u2028\u2029\uFEFF';

  var whitespace = '[' + whitespaces + ']';
  var ltrim = RegExp('^' + whitespace + whitespace + '*');
  var rtrim = RegExp(whitespace + whitespace + '*$');

  var stringTrim = function (string, TYPE) {
    string = String(requireObjectCoercible(string));
    if (TYPE & 1) string = string.replace(ltrim, '');
    if (TYPE & 2) string = string.replace(rtrim, '');
    return string;
  };

  var nativeParseInt = global$1.parseInt;


  var hex = /^[-+]?0[xX]/;
  var FORCED = nativeParseInt(whitespaces + '08') !== 8 || nativeParseInt(whitespaces + '0x16') !== 22;

  var _parseInt = FORCED ? function parseInt(str, radix) {
    var string = stringTrim(String(str), 3);
    return nativeParseInt(string, (radix >>> 0) || (hex.test(string) ? 16 : 10));
  } : nativeParseInt;

  _export({ global: true, forced: parseInt != _parseInt }, {
    parseInt: _parseInt
  });

  var regexpFlags = function () {
    var that = anObject(this);
    var result = '';
    if (that.global) result += 'g';
    if (that.ignoreCase) result += 'i';
    if (that.multiline) result += 'm';
    if (that.unicode) result += 'u';
    if (that.sticky) result += 'y';
    return result;
  };

  var nativeExec = RegExp.prototype.exec;

  var nativeReplace = String.prototype.replace;

  var patchedExec = nativeExec;

  var UPDATES_LAST_INDEX_WRONG = (function () {
    var re1 = /a/;
    var re2 = /b*/g;
    nativeExec.call(re1, 'a');
    nativeExec.call(re2, 'a');
    return re1.lastIndex !== 0 || re2.lastIndex !== 0;
  })();
  var NPCG_INCLUDED = /()??/.exec('')[1] !== undefined;

  var PATCH = UPDATES_LAST_INDEX_WRONG || NPCG_INCLUDED;

  if (PATCH) {
    patchedExec = function exec(str) {
      var re = this;
      var lastIndex, reCopy, match, i;

      if (NPCG_INCLUDED) {
        reCopy = new RegExp('^' + re.source + '$(?!\\s)', regexpFlags.call(re));
      }
      if (UPDATES_LAST_INDEX_WRONG) lastIndex = re.lastIndex;

      match = nativeExec.call(re, str);

      if (UPDATES_LAST_INDEX_WRONG && match) {
        re.lastIndex = re.global ? match.index + match[0].length : lastIndex;
      }
      if (NPCG_INCLUDED && match && match.length > 1) {
        // for NPCG, like IE8. NOTE: This doesn' work for /(.?)?/
        nativeReplace.call(match[0], reCopy, function () {
          for (i = 1; i < arguments.length - 2; i++) {
            if (arguments[i] === undefined) match[i] = undefined;
          }
        });
      }

      return match;
    };
  }

  var regexpExec = patchedExec;

  _export({ target: 'RegExp', proto: true, forced: /./.exec !== regexpExec }, {
    exec: regexpExec
  });
  // CONVERT_TO_STRING: false -> String#codePointAt
  var stringAt = function (that, pos, CONVERT_TO_STRING) {
    var S = String(requireObjectCoercible(that));
    var position = toInteger(pos);
    var size = S.length;
    var first, second;
    if (position < 0 || position >= size) return CONVERT_TO_STRING ? '' : undefined;
    first = S.charCodeAt(position);
    return first < 0xD800 || first > 0xDBFF || position + 1 === size
      || (second = S.charCodeAt(position + 1)) < 0xDC00 || second > 0xDFFF
        ? CONVERT_TO_STRING ? S.charAt(position) : first
        : CONVERT_TO_STRING ? S.slice(position, position + 2) : (first - 0xD800 << 10) + (second - 0xDC00) + 0x10000;
  };

  var STRING_ITERATOR = 'String Iterator';
  var setInternalState$1 = internalState.set;
  var getInternalState$1 = internalState.getterFor(STRING_ITERATOR);
  // https://tc39.github.io/ecma262/#sec-string.prototype-@@iterator
  defineIterator(String, 'String', function (iterated) {
    setInternalState$1(this, {
      type: STRING_ITERATOR,
      string: String(iterated),
      index: 0
    });
  // https://tc39.github.io/ecma262/#sec-%stringiteratorprototype%.next
  }, function next() {
    var state = getInternalState$1(this);
    var string = state.string;
    var index = state.index;
    var point;
    if (index >= string.length) return { value: undefined, done: true };
    point = stringAt(string, index, true);
    state.index += point.length;
    return { value: point, done: false };
  });
  // https://tc39.github.io/ecma262/#sec-advancestringindex
  var advanceStringIndex = function (S, index, unicode) {
    return index + (unicode ? stringAt(S, index, true).length : 1);
  };
  var regexpExecAbstract = function (R, S) {
    var exec = R.exec;
    if (typeof exec === 'function') {
      var result = exec.call(R, S);
      if (typeof result !== 'object') {
        throw TypeError('RegExp exec method returned something other than an Object or null');
      }
      return result;
    }

    if (classofRaw(R) !== 'RegExp') {
      throw TypeError('RegExp#exec called on incompatible receiver');
    }

    return regexpExec.call(R, S);
  };

  var SPECIES$2 = wellKnownSymbol('species');

  var REPLACE_SUPPORTS_NAMED_GROUPS = !fails(function () {
    // #match works fine because it just return the exec results, even if it has
    var re = /./;
    re.exec = function () {
      var result = [];
      result.groups = { a: '7' };
      return result;
    };
    return ''.replace(re, '$<a>') !== '7';
  });
  // Weex JS has frozen built-in prototypes, so use try / catch wrapper
  var SPLIT_WORKS_WITH_OVERWRITTEN_EXEC = !fails(function () {
    var re = /(?:)/;
    var originalExec = re.exec;
    re.exec = function () { return originalExec.apply(this, arguments); };
    var result = 'ab'.split(re);
    return result.length !== 2 || result[0] !== 'a' || result[1] !== 'b';
  });

  var fixRegexpWellKnownSymbolLogic = function (KEY, length, exec, sham) {
    var SYMBOL = wellKnownSymbol(KEY);

    var DELEGATES_TO_SYMBOL = !fails(function () {
      var O = {};
      O[SYMBOL] = function () { return 7; };
      return ''[KEY](O) != 7;
    });

    var DELEGATES_TO_EXEC = DELEGATES_TO_SYMBOL && !fails(function () {
      var execCalled = false;
      var re = /a/;
      re.exec = function () { execCalled = true; return null; };

      if (KEY === 'split') {
        // a new one. We need to return the patched regex when creating the new one.
        re.constructor = {};
        re.constructor[SPECIES$2] = function () { return re; };
      }

      re[SYMBOL]('');
      return !execCalled;
    });

    if (
      !DELEGATES_TO_SYMBOL ||
      !DELEGATES_TO_EXEC ||
      (KEY === 'replace' && !REPLACE_SUPPORTS_NAMED_GROUPS) ||
      (KEY === 'split' && !SPLIT_WORKS_WITH_OVERWRITTEN_EXEC)
    ) {
      var nativeRegExpMethod = /./[SYMBOL];
      var methods = exec(SYMBOL, ''[KEY], function (nativeMethod, regexp, str, arg2, forceStringMethod) {
        if (regexp.exec === regexpExec) {
          if (DELEGATES_TO_SYMBOL && !forceStringMethod) {
            // polyfilled function), leasing to infinite recursion.
            return { done: true, value: nativeRegExpMethod.call(regexp, str, arg2) };
          }
          return { done: true, value: nativeMethod.call(str, regexp, arg2) };
        }
        return { done: false };
      });
      var stringMethod = methods[0];
      var regexMethod = methods[1];

      redefine(String.prototype, KEY, stringMethod);
      redefine(RegExp.prototype, SYMBOL, length == 2
        // 21.2.5.11 RegExp.prototype[@@split](string, limit)
        ? function (string, arg) { return regexMethod.call(string, this, arg); }
        // 21.2.5.9 RegExp.prototype[@@search](string)
        : function (string) { return regexMethod.call(string, this); }
      );
      if (sham) hide(RegExp.prototype[SYMBOL], 'sham', true);
    }
  };
  fixRegexpWellKnownSymbolLogic(
    'match',
    1,
    function (MATCH, nativeMatch, maybeCallNative) {
      return [
        // https://tc39.github.io/ecma262/#sec-string.prototype.match
        function match(regexp) {
          var O = requireObjectCoercible(this);
          var matcher = regexp == undefined ? undefined : regexp[MATCH];
          return matcher !== undefined ? matcher.call(regexp, O) : new RegExp(regexp)[MATCH](String(O));
        },
        // https://tc39.github.io/ecma262/#sec-regexp.prototype-@@match
        function (regexp) {
          var res = maybeCallNative(nativeMatch, regexp, this);
          if (res.done) return res.value;

          var rx = anObject(regexp);
          var S = String(this);

          if (!rx.global) return regexpExecAbstract(rx, S);

          var fullUnicode = rx.unicode;
          rx.lastIndex = 0;
          var A = [];
          var n = 0;
          var result;
          while ((result = regexpExecAbstract(rx, S)) !== null) {
            var matchStr = String(result[0]);
            A[n] = matchStr;
            if (matchStr === '') rx.lastIndex = advanceStringIndex(S, toLength(rx.lastIndex), fullUnicode);
            n++;
          }
          return n === 0 ? null : A;
        }
      ];
    }
  );

  var max$1 = Math.max;
  var min$2 = Math.min;
  var floor$1 = Math.floor;
  var SUBSTITUTION_SYMBOLS = /\$([$&`']|\d\d?|<[^>]*>)/g;
  var SUBSTITUTION_SYMBOLS_NO_NAMED = /\$([$&`']|\d\d?)/g;

  var maybeToString = function (it) {
    return it === undefined ? it : String(it);
  };
  fixRegexpWellKnownSymbolLogic(
    'replace',
    2,
    function (REPLACE, nativeReplace, maybeCallNative) {
      return [
        // https://tc39.github.io/ecma262/#sec-string.prototype.replace
        function replace(searchValue, replaceValue) {
          var O = requireObjectCoercible(this);
          var replacer = searchValue == undefined ? undefined : searchValue[REPLACE];
          return replacer !== undefined
            ? replacer.call(searchValue, O, replaceValue)
            : nativeReplace.call(String(O), searchValue, replaceValue);
        },
        // https://tc39.github.io/ecma262/#sec-regexp.prototype-@@replace
        function (regexp, replaceValue) {
          var res = maybeCallNative(nativeReplace, regexp, this, replaceValue);
          if (res.done) return res.value;

          var rx = anObject(regexp);
          var S = String(this);

          var functionalReplace = typeof replaceValue === 'function';
          if (!functionalReplace) replaceValue = String(replaceValue);

          var global = rx.global;
          if (global) {
            var fullUnicode = rx.unicode;
            rx.lastIndex = 0;
          }
          var results = [];
          while (true) {
            var result = regexpExecAbstract(rx, S);
            if (result === null) break;

            results.push(result);
            if (!global) break;

            var matchStr = String(result[0]);
            if (matchStr === '') rx.lastIndex = advanceStringIndex(S, toLength(rx.lastIndex), fullUnicode);
          }

          var accumulatedResult = '';
          var nextSourcePosition = 0;
          for (var i = 0; i < results.length; i++) {
            result = results[i];

            var matched = String(result[0]);
            var position = max$1(min$2(toInteger(result.index), S.length), 0);
            var captures = [];
            //   captures = result.slice(1).map(maybeToString)
            // the slice polyfill when slicing native arrays) "doesn't work" in safari 9 and
            for (var j = 1; j < result.length; j++) captures.push(maybeToString(result[j]));
            var namedCaptures = result.groups;
            if (functionalReplace) {
              var replacerArgs = [matched].concat(captures, position, S);
              if (namedCaptures !== undefined) replacerArgs.push(namedCaptures);
              var replacement = String(replaceValue.apply(undefined, replacerArgs));
            } else {
              replacement = getSubstitution(matched, S, position, captures, namedCaptures, replaceValue);
            }
            if (position >= nextSourcePosition) {
              accumulatedResult += S.slice(nextSourcePosition, position) + replacement;
              nextSourcePosition = position + matched.length;
            }
          }
          return accumulatedResult + S.slice(nextSourcePosition);
        }
      ];
      function getSubstitution(matched, str, position, captures, namedCaptures, replacement) {
        var tailPos = position + matched.length;
        var m = captures.length;
        var symbols = SUBSTITUTION_SYMBOLS_NO_NAMED;
        if (namedCaptures !== undefined) {
          namedCaptures = toObject(namedCaptures);
          symbols = SUBSTITUTION_SYMBOLS;
        }
        return nativeReplace.call(replacement, symbols, function (match, ch) {
          var capture;
          switch (ch.charAt(0)) {
            case '$': return '$';
            case '&': return matched;
            case '`': return str.slice(0, position);
            case "'": return str.slice(tailPos);
            case '<':
              capture = namedCaptures[ch.slice(1, -1)];
              break;
            default: // \d\d?
              var n = +ch;
              if (n === 0) return match;
              if (n > m) {
                var f = floor$1(n / 10);
                if (f === 0) return match;
                if (f <= m) return captures[f - 1] === undefined ? ch.charAt(1) : captures[f - 1] + ch.charAt(1);
                return match;
              }
              capture = captures[n - 1];
          }
          return capture === undefined ? '' : capture;
        });
      }
    }
  );

  var redefineAll = function (target, src, options) {
    for (var key in src) redefine(target, key, src[key], options);
    return target;
  };

  var freezing = !fails(function () {
    return Object.isExtensible(Object.preventExtensions({}));
  });

  var internalMetadata = createCommonjsModule(function (module) {
  var METADATA = uid('meta');



  var defineProperty = objectDefineProperty.f;
  var id = 0;

  var isExtensible = Object.isExtensible || function () {
    return true;
  };

  var setMetadata = function (it) {
    defineProperty(it, METADATA, { value: {
      objectID: 'O' + ++id, // object ID
      weakData: {}          // weak collections IDs
    } });
  };

  var fastKey = function (it, create) {
    if (!isObject(it)) return typeof it == 'symbol' ? it : (typeof it == 'string' ? 'S' : 'P') + it;
    if (!has(it, METADATA)) {
      if (!isExtensible(it)) return 'F';
      if (!create) return 'E';
      setMetadata(it);
    } return it[METADATA].objectID;
  };

  var getWeakData = function (it, create) {
    if (!has(it, METADATA)) {
      if (!isExtensible(it)) return true;
      if (!create) return false;
      setMetadata(it);
    } return it[METADATA].weakData;
  };
  var onFreeze = function (it) {
    if (freezing && meta.REQUIRED && isExtensible(it) && !has(it, METADATA)) setMetadata(it);
    return it;
  };

  var meta = module.exports = {
    REQUIRED: false,
    fastKey: fastKey,
    getWeakData: getWeakData,
    onFreeze: onFreeze
  };

  hiddenKeys[METADATA] = true;
  });
  var internalMetadata_1 = internalMetadata.REQUIRED;
  var internalMetadata_2 = internalMetadata.fastKey;
  var internalMetadata_3 = internalMetadata.getWeakData;
  var internalMetadata_4 = internalMetadata.onFreeze;

  var anInstance = function (it, Constructor, name) {
    if (!(it instanceof Constructor)) {
      throw TypeError('Incorrect ' + (name ? name + ' ' : '') + 'invocation');
    } return it;
  };

  var ITERATOR$2 = wellKnownSymbol('iterator');
  var ArrayPrototype$1 = Array.prototype;

  var isArrayIteratorMethod = function (it) {
    return it !== undefined && (iterators.Array === it || ArrayPrototype$1[ITERATOR$2] === it);
  };

  var ITERATOR$3 = wellKnownSymbol('iterator');


  var getIteratorMethod = function (it) {
    if (it != undefined) return it[ITERATOR$3]
      || it['@@iterator']
      || iterators[classof(it)];
  };
  var callWithSafeIterationClosing = function (iterator, fn, value, ENTRIES) {
    try {
      return ENTRIES ? fn(anObject(value)[0], value[1]) : fn(value);
    } catch (error) {
      var returnMethod = iterator['return'];
      if (returnMethod !== undefined) anObject(returnMethod.call(iterator));
      throw error;
    }
  };

  var iterate = createCommonjsModule(function (module) {
  var BREAK = {};

  var exports = module.exports = function (iterable, fn, that, ENTRIES, ITERATOR) {
    var boundFunction = bindContext(fn, that, ENTRIES ? 2 : 1);
    var iterator, iterFn, index, length, result, step;

    if (ITERATOR) {
      iterator = iterable;
    } else {
      iterFn = getIteratorMethod(iterable);
      if (typeof iterFn != 'function') throw TypeError('Target is not iterable');
      if (isArrayIteratorMethod(iterFn)) {
        for (index = 0, length = toLength(iterable.length); length > index; index++) {
          result = ENTRIES ? boundFunction(anObject(step = iterable[index])[0], step[1]) : boundFunction(iterable[index]);
          if (result === BREAK) return BREAK;
        } return;
      }
      iterator = iterFn.call(iterable);
    }

    while (!(step = iterator.next()).done) {
      if (callWithSafeIterationClosing(iterator, boundFunction, step.value, ENTRIES) === BREAK) return BREAK;
    }
  };

  exports.BREAK = BREAK;
  });

  var getWeakData = internalMetadata.getWeakData;







  var setInternalState$2 = internalState.set;
  var internalStateGetterFor = internalState.getterFor;
  var arrayFind = arrayMethods(5);
  var arrayFindIndex = arrayMethods(6);
  var id$1 = 0;
  var uncaughtFrozenStore = function (store) {
    return store.frozen || (store.frozen = new UncaughtFrozenStore());
  };

  var UncaughtFrozenStore = function () {
    this.entries = [];
  };

  var findUncaughtFrozen = function (store, key) {
    return arrayFind(store.entries, function (it) {
      return it[0] === key;
    });
  };

  UncaughtFrozenStore.prototype = {
    get: function (key) {
      var entry = findUncaughtFrozen(this, key);
      if (entry) return entry[1];
    },
    has: function (key) {
      return !!findUncaughtFrozen(this, key);
    },
    set: function (key, value) {
      var entry = findUncaughtFrozen(this, key);
      if (entry) entry[1] = value;
      else this.entries.push([key, value]);
    },
    'delete': function (key) {
      var index = arrayFindIndex(this.entries, function (it) {
        return it[0] === key;
      });
      if (~index) this.entries.splice(index, 1);
      return !!~index;
    }
  };

  var collectionWeak = {
    getConstructor: function (wrapper, CONSTRUCTOR_NAME, IS_MAP, ADDER) {
      var C = wrapper(function (that, iterable) {
        anInstance(that, C, CONSTRUCTOR_NAME);
        setInternalState$2(that, {
          type: CONSTRUCTOR_NAME,
          id: id$1++,
          frozen: undefined
        });
        if (iterable != undefined) iterate(iterable, that[ADDER], that, IS_MAP);
      });

      var getInternalState = internalStateGetterFor(CONSTRUCTOR_NAME);

      var define = function (that, key, value) {
        var state = getInternalState(that);
        var data = getWeakData(anObject(key), true);
        if (data === true) uncaughtFrozenStore(state).set(key, value);
        else data[state.id] = value;
        return that;
      };

      redefineAll(C.prototype, {
     
        'delete': function (key) {
          var state = getInternalState(this);
          if (!isObject(key)) return false;
          var data = getWeakData(key);
          if (data === true) return uncaughtFrozenStore(state)['delete'](key);
          return data && has(data, state.id) && delete data[state.id];
        },
        
        has: function has$1(key) {
          var state = getInternalState(this);
          if (!isObject(key)) return false;
          var data = getWeakData(key);
          if (data === true) return uncaughtFrozenStore(state).has(key);
          return data && has(data, state.id);
        }
      });

      redefineAll(C.prototype, IS_MAP ? {
        get: function get(key) {
          var state = getInternalState(this);
          if (isObject(key)) {
            var data = getWeakData(key);
            if (data === true) return uncaughtFrozenStore(state).get(key);
            return data ? data[state.id] : undefined;
          }
        },
        set: function set(key, value) {
          return define(this, key, value);
        }
      } : {
        add: function add(value) {
          return define(this, value, true);
        }
      });

      return C;
    }
  };

  var ITERATOR$4 = wellKnownSymbol('iterator');
  var SAFE_CLOSING = false;

  try {
    var called = 0;
    var iteratorWithReturn = {
      next: function () {
        return { done: !!called++ };
      },
      'return': function () {
        SAFE_CLOSING = true;
      }
    };
    iteratorWithReturn[ITERATOR$4] = function () {
      return this;
    };
  } catch (error) { /* empty */ }

  var checkCorrectnessOfIteration = function (exec, SKIP_CLOSING) {
    if (!SKIP_CLOSING && !SAFE_CLOSING) return false;
    var ITERATION_SUPPORT = false;
    try {
      var object = {};
      object[ITERATOR$4] = function () {
        return {
          next: function () {
            return { done: ITERATION_SUPPORT = true };
          }
        };
      };
      exec(object);
    } catch (error) { /* empty */ }
    return ITERATION_SUPPORT;
  };

  var inheritIfRequired = function (that, target, C) {
    var S = target.constructor;
    var P;
    if (S !== C && typeof S == 'function' && (P = S.prototype) !== C.prototype && isObject(P) && objectSetPrototypeOf) {
      objectSetPrototypeOf(that, P);
    } return that;
  };

  var collection = function (CONSTRUCTOR_NAME, wrapper, common, IS_MAP, IS_WEAK) {
    var NativeConstructor = global$1[CONSTRUCTOR_NAME];
    var NativePrototype = NativeConstructor && NativeConstructor.prototype;
    var Constructor = NativeConstructor;
    var ADDER = IS_MAP ? 'set' : 'add';
    var exported = {};

    var fixMethod = function (KEY) {
      var nativeMethod = NativePrototype[KEY];
      redefine(NativePrototype, KEY,
        KEY == 'add' ? function add(a) {
          nativeMethod.call(this, a === 0 ? 0 : a);
          return this;
        } : KEY == 'delete' ? function (a) {
          return IS_WEAK && !isObject(a) ? false : nativeMethod.call(this, a === 0 ? 0 : a);
        } : KEY == 'get' ? function get(a) {
          return IS_WEAK && !isObject(a) ? undefined : nativeMethod.call(this, a === 0 ? 0 : a);
        } : KEY == 'has' ? function has(a) {
          return IS_WEAK && !isObject(a) ? false : nativeMethod.call(this, a === 0 ? 0 : a);
        } : function set(a, b) {
          nativeMethod.call(this, a === 0 ? 0 : a, b);
          return this;
        }
      );
    };
    if (isForced_1(CONSTRUCTOR_NAME, typeof NativeConstructor != 'function' || !(IS_WEAK || NativePrototype.forEach && !fails(function () {
      new NativeConstructor().entries().next();
    })))) {
      Constructor = common.getConstructor(wrapper, CONSTRUCTOR_NAME, IS_MAP, ADDER);
      internalMetadata.REQUIRED = true;
    } else if (isForced_1(CONSTRUCTOR_NAME, true)) {
      var instance = new Constructor();
      var HASNT_CHAINING = instance[ADDER](IS_WEAK ? {} : -0, 1) != instance;
      var THROWS_ON_PRIMITIVES = fails(function () { instance.has(1); });
      // eslint-disable-next-line no-new
      var ACCEPT_ITERABLES = checkCorrectnessOfIteration(function (iterable) { new NativeConstructor(iterable); });
      var BUGGY_ZERO = !IS_WEAK && fails(function () {
        var $instance = new NativeConstructor();
        var index = 5;
        while (index--) $instance[ADDER](index, index);
        return !$instance.has(-0);
      });

      if (!ACCEPT_ITERABLES) {
        Constructor = wrapper(function (target, iterable) {
          anInstance(target, Constructor, CONSTRUCTOR_NAME);
          var that = inheritIfRequired(new NativeConstructor(), target, Constructor);
          if (iterable != undefined) iterate(iterable, that[ADDER], that, IS_MAP);
          return that;
        });
        Constructor.prototype = NativePrototype;
        NativePrototype.constructor = Constructor;
      }

      if (THROWS_ON_PRIMITIVES || BUGGY_ZERO) {
        fixMethod('delete');
        fixMethod('has');
        IS_MAP && fixMethod('get');
      }

      if (BUGGY_ZERO || HASNT_CHAINING) fixMethod(ADDER);
      if (IS_WEAK && NativePrototype.clear) delete NativePrototype.clear;
    }

    exported[CONSTRUCTOR_NAME] = Constructor;
    _export({ global: true, forced: Constructor != NativeConstructor }, exported);

    setToStringTag(Constructor, CONSTRUCTOR_NAME);

    if (!IS_WEAK) common.setStrong(Constructor, CONSTRUCTOR_NAME, IS_MAP);

    return Constructor;
  };

  var es_weakMap = createCommonjsModule(function (module) {





  var enforceIternalState = internalState.enforce;

  var IS_IE11 = !global$1.ActiveXObject && 'ActiveXObject' in global$1;
  var isExtensible = Object.isExtensible;
  var InternalWeakMap;

  var wrapper = function (get) {
    return function WeakMap() {
      return get(this, arguments.length > 0 ? arguments[0] : undefined);
    };
  };
  // https://tc39.github.io/ecma262/#sec-weakmap-constructor
  var $WeakMap = module.exports = collection('WeakMap', wrapper, collectionWeak, true, true);
  // We can't use feature detection because it crash some old IE builds
  if (nativeWeakMap && IS_IE11) {
    InternalWeakMap = collectionWeak.getConstructor(wrapper, 'WeakMap', true);
    internalMetadata.REQUIRED = true;
    var WeakMapPrototype = $WeakMap.prototype;
    var nativeDelete = WeakMapPrototype['delete'];
    var nativeHas = WeakMapPrototype.has;
    var nativeGet = WeakMapPrototype.get;
    var nativeSet = WeakMapPrototype.set;
    redefineAll(WeakMapPrototype, {
      'delete': function (key) {
        if (isObject(key) && !isExtensible(key)) {
          var state = enforceIternalState(this);
          if (!state.frozen) state.frozen = new InternalWeakMap();
          return nativeDelete.call(this, key) || state.frozen['delete'](key);
        } return nativeDelete.call(this, key);
      },
      has: function has(key) {
        if (isObject(key) && !isExtensible(key)) {
          var state = enforceIternalState(this);
          if (!state.frozen) state.frozen = new InternalWeakMap();
          return nativeHas.call(this, key) || state.frozen.has(key);
        } return nativeHas.call(this, key);
      },
      get: function get(key) {
        if (isObject(key) && !isExtensible(key)) {
          var state = enforceIternalState(this);
          if (!state.frozen) state.frozen = new InternalWeakMap();
          return nativeHas.call(this, key) ? nativeGet.call(this, key) : state.frozen.get(key);
        } return nativeGet.call(this, key);
      },
      set: function set(key, value) {
        if (isObject(key) && !isExtensible(key)) {
          var state = enforceIternalState(this);
          if (!state.frozen) state.frozen = new InternalWeakMap();
          nativeHas.call(this, key) ? nativeSet.call(this, key, value) : state.frozen.set(key, value);
        } else nativeSet.call(this, key, value);
        return this;
      }
    });
  }
  });
  // flag - `iterable` interface - 'entries', 'keys', 'values', 'forEach' methods
  var domIterables = {
    CSSRuleList: 0,
    CSSStyleDeclaration: 0,
    CSSValueList: 0,
    ClientRectList: 0,
    DOMRectList: 0,
    DOMStringList: 0,
    DOMTokenList: 1,
    DataTransferItemList: 0,
    FileList: 0,
    HTMLAllCollection: 0,
    HTMLCollection: 0,
    HTMLFormElement: 0,
    HTMLSelectElement: 0,
    MediaList: 0,
    MimeTypeArray: 0,
    NamedNodeMap: 0,
    NodeList: 1,
    PaintRequestList: 0,
    Plugin: 0,
    PluginArray: 0,
    SVGLengthList: 0,
    SVGNumberList: 0,
    SVGPathSegList: 0,
    SVGPointList: 0,
    SVGStringList: 0,
    SVGTransformList: 0,
    SourceBufferList: 0,
    StyleSheetList: 0,
    TextTrackCueList: 0,
    TextTrackList: 0,
    TouchList: 0
  };

  for (var COLLECTION_NAME in domIterables) {
    var Collection = global$1[COLLECTION_NAME];
    var CollectionPrototype = Collection && Collection.prototype;
    if (CollectionPrototype && CollectionPrototype.forEach !== arrayForEach) try {
      hide(CollectionPrototype, 'forEach', arrayForEach);
    } catch (error) {
      CollectionPrototype.forEach = arrayForEach;
    }
  }

  var ITERATOR$5 = wellKnownSymbol('iterator');
  var TO_STRING_TAG$3 = wellKnownSymbol('toStringTag');
  var ArrayValues = es_array_iterator.values;

  for (var COLLECTION_NAME$1 in domIterables) {
    var Collection$1 = global$1[COLLECTION_NAME$1];
    var CollectionPrototype$1 = Collection$1 && Collection$1.prototype;
    if (CollectionPrototype$1) {
      if (CollectionPrototype$1[ITERATOR$5] !== ArrayValues) try {
        hide(CollectionPrototype$1, ITERATOR$5, ArrayValues);
      } catch (error) {
        CollectionPrototype$1[ITERATOR$5] = ArrayValues;
      }
      if (!CollectionPrototype$1[TO_STRING_TAG$3]) hide(CollectionPrototype$1, TO_STRING_TAG$3, COLLECTION_NAME$1);
      if (domIterables[COLLECTION_NAME$1]) for (var METHOD_NAME in es_array_iterator) {
        if (CollectionPrototype$1[METHOD_NAME] !== es_array_iterator[METHOD_NAME]) try {
          hide(CollectionPrototype$1, METHOD_NAME, es_array_iterator[METHOD_NAME]);
        } catch (error) {
          CollectionPrototype$1[METHOD_NAME] = es_array_iterator[METHOD_NAME];
        }
      }
    }
  }
  /** Used as the `TypeError` message for "Functions" methods. */
  var FUNC_ERROR_TEXT = 'Expected a function';
  var NAN = 0 / 0;
  var symbolTag = '[object Symbol]';
  var reTrim = /^\s+|\s+$/g;
  var reIsBadHex = /^[-+]0x[0-9a-f]+$/i;
  var reIsBinary = /^0b[01]+$/i;
  var reIsOctal = /^0o[0-7]+$/i;
  var freeParseInt = parseInt;
  var freeGlobal = typeof commonjsGlobal == 'object' && commonjsGlobal && commonjsGlobal.Object === Object && commonjsGlobal;
  var freeSelf = typeof self == 'object' && self && self.Object === Object && self;
  var root = freeGlobal || freeSelf || Function('return this')();
  var objectProto = Object.prototype;
  var objectToString$1 = objectProto.toString;
  var nativeMax = Math.max,
      nativeMin = Math.min;
  var now = function() {
    return root.Date.now();
  };
  function debounce(func, wait, options) {
    var lastArgs,
        lastThis,
        maxWait,
        result,
        timerId,
        lastCallTime,
        lastInvokeTime = 0,
        leading = false,
        maxing = false,
        trailing = true;

    if (typeof func != 'function') {
      throw new TypeError(FUNC_ERROR_TEXT);
    }
    wait = toNumber(wait) || 0;
    if (isObject$1(options)) {
      leading = !!options.leading;
      maxing = 'maxWait' in options;
      maxWait = maxing ? nativeMax(toNumber(options.maxWait) || 0, wait) : maxWait;
      trailing = 'trailing' in options ? !!options.trailing : trailing;
    }

    function invokeFunc(time) {
      var args = lastArgs,
          thisArg = lastThis;

      lastArgs = lastThis = undefined;
      lastInvokeTime = time;
      result = func.apply(thisArg, args);
      return result;
    }

    function leadingEdge(time) {
      lastInvokeTime = time;
      timerId = setTimeout(timerExpired, wait);
      return leading ? invokeFunc(time) : result;
    }

    function remainingWait(time) {
      var timeSinceLastCall = time - lastCallTime,
          timeSinceLastInvoke = time - lastInvokeTime,
          result = wait - timeSinceLastCall;

      return maxing ? nativeMin(result, maxWait - timeSinceLastInvoke) : result;
    }

    function shouldInvoke(time) {
      var timeSinceLastCall = time - lastCallTime,
          timeSinceLastInvoke = time - lastInvokeTime;
      // trailing edge, the system time has gone backwards and we're treating
      return (lastCallTime === undefined || (timeSinceLastCall >= wait) ||
        (timeSinceLastCall < 0) || (maxing && timeSinceLastInvoke >= maxWait));
    }

    function timerExpired() {
      var time = now();
      if (shouldInvoke(time)) {
        return trailingEdge(time);
      }
      timerId = setTimeout(timerExpired, remainingWait(time));
    }

    function trailingEdge(time) {
      timerId = undefined;
      // debounced at least once.
      if (trailing && lastArgs) {
        return invokeFunc(time);
      }
      lastArgs = lastThis = undefined;
      return result;
    }

    function cancel() {
      if (timerId !== undefined) {
        clearTimeout(timerId);
      }
      lastInvokeTime = 0;
      lastArgs = lastCallTime = lastThis = timerId = undefined;
    }

    function flush() {
      return timerId === undefined ? result : trailingEdge(now());
    }

    function debounced() {
      var time = now(),
          isInvoking = shouldInvoke(time);

      lastArgs = arguments;
      lastThis = this;
      lastCallTime = time;

      if (isInvoking) {
        if (timerId === undefined) {
          return leadingEdge(lastCallTime);
        }
        if (maxing) {
          timerId = setTimeout(timerExpired, wait);
          return invokeFunc(lastCallTime);
        }
      }
      if (timerId === undefined) {
        timerId = setTimeout(timerExpired, wait);
      }
      return result;
    }
    debounced.cancel = cancel;
    debounced.flush = flush;
    return debounced;
  }
  function throttle(func, wait, options) {
    var leading = true,
        trailing = true;

    if (typeof func != 'function') {
      throw new TypeError(FUNC_ERROR_TEXT);
    }
    if (isObject$1(options)) {
      leading = 'leading' in options ? !!options.leading : leading;
      trailing = 'trailing' in options ? !!options.trailing : trailing;
    }
    return debounce(func, wait, {
      'leading': leading,
      'maxWait': wait,
      'trailing': trailing
    });
  }
  function isObject$1(value) {
    var type = typeof value;
    return !!value && (type == 'object' || type == 'function');
  }
  function isObjectLike(value) {
    return !!value && typeof value == 'object';
  }
  function isSymbol(value) {
    return typeof value == 'symbol' ||
      (isObjectLike(value) && objectToString$1.call(value) == symbolTag);
  }
  function toNumber(value) {
    if (typeof value == 'number') {
      return value;
    }
    if (isSymbol(value)) {
      return NAN;
    }
    if (isObject$1(value)) {
      var other = typeof value.valueOf == 'function' ? value.valueOf() : value;
      value = isObject$1(other) ? (other + '') : other;
    }
    if (typeof value != 'string') {
      return value === 0 ? value : +value;
    }
    value = value.replace(reTrim, '');
    var isBinary = reIsBinary.test(value);
    return (isBinary || reIsOctal.test(value))
      ? freeParseInt(value.slice(2), isBinary ? 2 : 8)
      : (reIsBadHex.test(value) ? NAN : +value);
  }

  var lodash_throttle = throttle;
  /** Used as the `TypeError` message for "Functions" methods. */
  var FUNC_ERROR_TEXT$1 = 'Expected a function';
  var NAN$1 = 0 / 0;
  var symbolTag$1 = '[object Symbol]';
  var reTrim$1 = /^\s+|\s+$/g;
  var reIsBadHex$1 = /^[-+]0x[0-9a-f]+$/i;
  var reIsBinary$1 = /^0b[01]+$/i;
  var reIsOctal$1 = /^0o[0-7]+$/i;
  var freeParseInt$1 = parseInt;
  var freeGlobal$1 = typeof commonjsGlobal == 'object' && commonjsGlobal && commonjsGlobal.Object === Object && commonjsGlobal;
  var freeSelf$1 = typeof self == 'object' && self && self.Object === Object && self;
  var root$1 = freeGlobal$1 || freeSelf$1 || Function('return this')();
  var objectProto$1 = Object.prototype;
  var objectToString$2 = objectProto$1.toString;
  var nativeMax$1 = Math.max,
      nativeMin$1 = Math.min;
  var now$1 = function() {
    return root$1.Date.now();
  };
  function debounce$1(func, wait, options) {
    var lastArgs,
        lastThis,
        maxWait,
        result,
        timerId,
        lastCallTime,
        lastInvokeTime = 0,
        leading = false,
        maxing = false,
        trailing = true;

    if (typeof func != 'function') {
      throw new TypeError(FUNC_ERROR_TEXT$1);
    }
    wait = toNumber$1(wait) || 0;
    if (isObject$2(options)) {
      leading = !!options.leading;
      maxing = 'maxWait' in options;
      maxWait = maxing ? nativeMax$1(toNumber$1(options.maxWait) || 0, wait) : maxWait;
      trailing = 'trailing' in options ? !!options.trailing : trailing;
    }

    function invokeFunc(time) {
      var args = lastArgs,
          thisArg = lastThis;

      lastArgs = lastThis = undefined;
      lastInvokeTime = time;
      result = func.apply(thisArg, args);
      return result;
    }

    function leadingEdge(time) {
      lastInvokeTime = time;
      timerId = setTimeout(timerExpired, wait);
      return leading ? invokeFunc(time) : result;
    }

    function remainingWait(time) {
      var timeSinceLastCall = time - lastCallTime,
          timeSinceLastInvoke = time - lastInvokeTime,
          result = wait - timeSinceLastCall;

      return maxing ? nativeMin$1(result, maxWait - timeSinceLastInvoke) : result;
    }

    function shouldInvoke(time) {
      var timeSinceLastCall = time - lastCallTime,
          timeSinceLastInvoke = time - lastInvokeTime;
      // trailing edge, the system time has gone backwards and we're treating
      return (lastCallTime === undefined || (timeSinceLastCall >= wait) ||
        (timeSinceLastCall < 0) || (maxing && timeSinceLastInvoke >= maxWait));
    }

    function timerExpired() {
      var time = now$1();
      if (shouldInvoke(time)) {
        return trailingEdge(time);
      }
      timerId = setTimeout(timerExpired, remainingWait(time));
    }

    function trailingEdge(time) {
      timerId = undefined;
      // debounced at least once.
      if (trailing && lastArgs) {
        return invokeFunc(time);
      }
      lastArgs = lastThis = undefined;
      return result;
    }

    function cancel() {
      if (timerId !== undefined) {
        clearTimeout(timerId);
      }
      lastInvokeTime = 0;
      lastArgs = lastCallTime = lastThis = timerId = undefined;
    }

    function flush() {
      return timerId === undefined ? result : trailingEdge(now$1());
    }

    function debounced() {
      var time = now$1(),
          isInvoking = shouldInvoke(time);

      lastArgs = arguments;
      lastThis = this;
      lastCallTime = time;

      if (isInvoking) {
        if (timerId === undefined) {
          return leadingEdge(lastCallTime);
        }
        if (maxing) {
          timerId = setTimeout(timerExpired, wait);
          return invokeFunc(lastCallTime);
        }
      }
      if (timerId === undefined) {
        timerId = setTimeout(timerExpired, wait);
      }
      return result;
    }
    debounced.cancel = cancel;
    debounced.flush = flush;
    return debounced;
  }
  function isObject$2(value) {
    var type = typeof value;
    return !!value && (type == 'object' || type == 'function');
  }
  function isObjectLike$1(value) {
    return !!value && typeof value == 'object';
  }
  function isSymbol$1(value) {
    return typeof value == 'symbol' ||
      (isObjectLike$1(value) && objectToString$2.call(value) == symbolTag$1);
  }
  function toNumber$1(value) {
    if (typeof value == 'number') {
      return value;
    }
    if (isSymbol$1(value)) {
      return NAN$1;
    }
    if (isObject$2(value)) {
      var other = typeof value.valueOf == 'function' ? value.valueOf() : value;
      value = isObject$2(other) ? (other + '') : other;
    }
    if (typeof value != 'string') {
      return value === 0 ? value : +value;
    }
    value = value.replace(reTrim$1, '');
    var isBinary = reIsBinary$1.test(value);
    return (isBinary || reIsOctal$1.test(value))
      ? freeParseInt$1(value.slice(2), isBinary ? 2 : 8)
      : (reIsBadHex$1.test(value) ? NAN$1 : +value);
  }

  var lodash_debounce = debounce$1;
  /** Used as the `TypeError` message for "Functions" methods. */
  var FUNC_ERROR_TEXT$2 = 'Expected a function';
  var HASH_UNDEFINED = '__lodash_hash_undefined__';
  var funcTag = '[object Function]',
      genTag = '[object GeneratorFunction]';
  var reRegExpChar = /[\\^$.*+?()[\]{}|]/g;
  var reIsHostCtor = /^\[object .+?Constructor\]$/;
  var freeGlobal$2 = typeof commonjsGlobal == 'object' && commonjsGlobal && commonjsGlobal.Object === Object && commonjsGlobal;
  var freeSelf$2 = typeof self == 'object' && self && self.Object === Object && self;
  var root$2 = freeGlobal$2 || freeSelf$2 || Function('return this')();
  function getValue(object, key) {
    return object == null ? undefined : object[key];
  }
  function isHostObject(value) {
    // despite having improperly defined `toString` methods.
    var result = false;
    if (value != null && typeof value.toString != 'function') {
      try {
        result = !!(value + '');
      } catch (e) {}
    }
    return result;
  }
  var arrayProto = Array.prototype,
      funcProto = Function.prototype,
      objectProto$2 = Object.prototype;
  var coreJsData = root$2['__core-js_shared__'];
  var maskSrcKey = (function() {
    var uid = /[^.]+$/.exec(coreJsData && coreJsData.keys && coreJsData.keys.IE_PROTO || '');
    return uid ? ('Symbol(src)_1.' + uid) : '';
  }());
  var funcToString = funcProto.toString;
  var hasOwnProperty$1 = objectProto$2.hasOwnProperty;
  var objectToString$3 = objectProto$2.toString;
  var reIsNative = RegExp('^' +
    funcToString.call(hasOwnProperty$1).replace(reRegExpChar, '\\$&')
    .replace(/hasOwnProperty|(function).*?(?=\\\()| for .+?(?=\\\])/g, '$1.*?') + '$'
  );
  var splice = arrayProto.splice;
  var Map$1 = getNative(root$2, 'Map'),
      nativeCreate = getNative(Object, 'create');
  function Hash(entries) {
    var index = -1,
        length = entries ? entries.length : 0;

    this.clear();
    while (++index < length) {
      var entry = entries[index];
      this.set(entry[0], entry[1]);
    }
  }
  function hashClear() {
    this.__data__ = nativeCreate ? nativeCreate(null) : {};
  }
  function hashDelete(key) {
    return this.has(key) && delete this.__data__[key];
  }
  function hashGet(key) {
    var data = this.__data__;
    if (nativeCreate) {
      var result = data[key];
      return result === HASH_UNDEFINED ? undefined : result;
    }
    return hasOwnProperty$1.call(data, key) ? data[key] : undefined;
  }
  function hashHas(key) {
    var data = this.__data__;
    return nativeCreate ? data[key] !== undefined : hasOwnProperty$1.call(data, key);
  }
  function hashSet(key, value) {
    var data = this.__data__;
    data[key] = (nativeCreate && value === undefined) ? HASH_UNDEFINED : value;
    return this;
  }
  Hash.prototype.clear = hashClear;
  Hash.prototype['delete'] = hashDelete;
  Hash.prototype.get = hashGet;
  Hash.prototype.has = hashHas;
  Hash.prototype.set = hashSet;
  function ListCache(entries) {
    var index = -1,
        length = entries ? entries.length : 0;

    this.clear();
    while (++index < length) {
      var entry = entries[index];
      this.set(entry[0], entry[1]);
    }
  }
  function listCacheClear() {
    this.__data__ = [];
  }
  function listCacheDelete(key) {
    var data = this.__data__,
        index = assocIndexOf(data, key);

    if (index < 0) {
      return false;
    }
    var lastIndex = data.length - 1;
    if (index == lastIndex) {
      data.pop();
    } else {
      splice.call(data, index, 1);
    }
    return true;
  }
  function listCacheGet(key) {
    var data = this.__data__,
        index = assocIndexOf(data, key);

    return index < 0 ? undefined : data[index][1];
  }
  function listCacheHas(key) {
    return assocIndexOf(this.__data__, key) > -1;
  }
  function listCacheSet(key, value) {
    var data = this.__data__,
        index = assocIndexOf(data, key);

    if (index < 0) {
      data.push([key, value]);
    } else {
      data[index][1] = value;
    }
    return this;
  }
  ListCache.prototype.clear = listCacheClear;
  ListCache.prototype['delete'] = listCacheDelete;
  ListCache.prototype.get = listCacheGet;
  ListCache.prototype.has = listCacheHas;
  ListCache.prototype.set = listCacheSet;
  function MapCache(entries) {
    var index = -1,
        length = entries ? entries.length : 0;

    this.clear();
    while (++index < length) {
      var entry = entries[index];
      this.set(entry[0], entry[1]);
    }
  }
  function mapCacheClear() {
    this.__data__ = {
      'hash': new Hash,
      'map': new (Map$1 || ListCache),
      'string': new Hash
    };
  }
  function mapCacheDelete(key) {
    return getMapData(this, key)['delete'](key);
  }
  function mapCacheGet(key) {
    return getMapData(this, key).get(key);
  }
  function mapCacheHas(key) {
    return getMapData(this, key).has(key);
  }
  function mapCacheSet(key, value) {
    getMapData(this, key).set(key, value);
    return this;
  }
  MapCache.prototype.clear = mapCacheClear;
  MapCache.prototype['delete'] = mapCacheDelete;
  MapCache.prototype.get = mapCacheGet;
  MapCache.prototype.has = mapCacheHas;
  MapCache.prototype.set = mapCacheSet;
  function assocIndexOf(array, key) {
    var length = array.length;
    while (length--) {
      if (eq(array[length][0], key)) {
        return length;
      }
    }
    return -1;
  }
  function baseIsNative(value) {
    if (!isObject$3(value) || isMasked(value)) {
      return false;
    }
    var pattern = (isFunction(value) || isHostObject(value)) ? reIsNative : reIsHostCtor;
    return pattern.test(toSource(value));
  }
  function getMapData(map, key) {
    var data = map.__data__;
    return isKeyable(key)
      ? data[typeof key == 'string' ? 'string' : 'hash']
      : data.map;
  }
  function getNative(object, key) {
    var value = getValue(object, key);
    return baseIsNative(value) ? value : undefined;
  }
  function isKeyable(value) {
    var type = typeof value;
    return (type == 'string' || type == 'number' || type == 'symbol' || type == 'boolean')
      ? (value !== '__proto__')
      : (value === null);
  }
  function isMasked(func) {
    return !!maskSrcKey && (maskSrcKey in func);
  }
  function toSource(func) {
    if (func != null) {
      try {
        return funcToString.call(func);
      } catch (e) {}
      try {
        return (func + '');
      } catch (e) {}
    }
    return '';
  }
  function memoize(func, resolver) {
    if (typeof func != 'function' || (resolver && typeof resolver != 'function')) {
      throw new TypeError(FUNC_ERROR_TEXT$2);
    }
    var memoized = function() {
      var args = arguments,
          key = resolver ? resolver.apply(this, args) : args[0],
          cache = memoized.cache;

      if (cache.has(key)) {
        return cache.get(key);
      }
      var result = func.apply(this, args);
      memoized.cache = cache.set(key, result);
      return result;
    };
    memoized.cache = new (memoize.Cache || MapCache);
    return memoized;
  }
  memoize.Cache = MapCache;
  function eq(value, other) {
    return value === other || (value !== value && other !== other);
  }
  function isFunction(value) {
    // in Safari 8-9 which returns 'object' for typed array and other constructors.
    var tag = isObject$3(value) ? objectToString$3.call(value) : '';
    return tag == funcTag || tag == genTag;
  }
  function isObject$3(value) {
    var type = typeof value;
    return !!value && (type == 'object' || type == 'function');
  }

  var lodash_memoize = memoize;
  /* eslint-disable require-jsdoc, valid-jsdoc */
  var MapShim = (function () {
      if (typeof Map !== 'undefined') {
          return Map;
      }
      function getIndex(arr, key) {
          var result = -1;
          arr.some(function (entry, index) {
              if (entry[0] === key) {
                  result = index;
                  return true;
              }
              return false;
          });
          return result;
      }
      return /** @class */ (function () {
          function class_1() {
              this.__entries__ = [];
          }
          Object.defineProperty(class_1.prototype, "size", {
              get: function () {
                  return this.__entries__.length;
              },
              enumerable: true,
              configurable: true
          });
          class_1.prototype.get = function (key) {
              var index = getIndex(this.__entries__, key);
              var entry = this.__entries__[index];
              return entry && entry[1];
          };
          class_1.prototype.set = function (key, value) {
              var index = getIndex(this.__entries__, key);
              if (~index) {
                  this.__entries__[index][1] = value;
              }
              else {
                  this.__entries__.push([key, value]);
              }
          };
          class_1.prototype.delete = function (key) {
              var entries = this.__entries__;
              var index = getIndex(entries, key);
              if (~index) {
                  entries.splice(index, 1);
              }
          };
          class_1.prototype.has = function (key) {
              return !!~getIndex(this.__entries__, key);
          };
          class_1.prototype.clear = function () {
              this.__entries__.splice(0);
          };
          class_1.prototype.forEach = function (callback, ctx) {
              if (ctx === void 0) { ctx = null; }
              for (var _i = 0, _a = this.__entries__; _i < _a.length; _i++) {
                  var entry = _a[_i];
                  callback.call(ctx, entry[1], entry[0]);
              }
          };
          return class_1;
      }());
  })();
  var isBrowser = typeof window !== 'undefined' && typeof document !== 'undefined' && window.document === document;
  var global$1$1 = (function () {
      if (typeof global !== 'undefined' && global.Math === Math) {
          return global;
      }
      if (typeof self !== 'undefined' && self.Math === Math) {
          return self;
      }
      if (typeof window !== 'undefined' && window.Math === Math) {
          return window;
      }
      return Function('return this')();
  })();
  var requestAnimationFrame$1 = (function () {
      if (typeof requestAnimationFrame === 'function') {
          // an "Invalid calling object" error if rAF is invoked without the global
          return requestAnimationFrame.bind(global$1$1);
      }
      return function (callback) { return setTimeout(function () { return callback(Date.now()); }, 1000 / 60); };
  })();
  var trailingTimeout = 2;
  function throttle$1 (callback, delay) {
      var leadingCall = false, trailingCall = false, lastCallTime = 0;
      function resolvePending() {
          if (leadingCall) {
              leadingCall = false;
              callback();
          }
          if (trailingCall) {
              proxy();
          }
      }
      function timeoutCallback() {
          requestAnimationFrame$1(resolvePending);
      }
      function proxy() {
          var timeStamp = Date.now();
          if (leadingCall) {
              if (timeStamp - lastCallTime < trailingTimeout) {
                  return;
              }
              // This is important for "transitions" which never actually start
              // happens amids the pending invocation.
              trailingCall = true;
          }
          else {
              leadingCall = true;
              trailingCall = false;
              setTimeout(timeoutCallback, delay);
          }
          lastCallTime = timeStamp;
      }
      return proxy;
  }
  var REFRESH_DELAY = 20;
  // might affect dimensions of observed elements.
  var transitionKeys = ['top', 'right', 'bottom', 'left', 'width', 'height', 'size', 'weight'];
  var mutationObserverSupported = typeof MutationObserver !== 'undefined';
  var ResizeObserverController = /** @class */ (function () {
      function ResizeObserverController() {
          this.connected_ = false;
          this.mutationEventsAdded_ = false;
          this.mutationsObserver_ = null;
          this.observers_ = [];
          this.onTransitionEnd_ = this.onTransitionEnd_.bind(this);
          this.refresh = throttle$1(this.refresh.bind(this), REFRESH_DELAY);
      }
      ResizeObserverController.prototype.addObserver = function (observer) {
          if (!~this.observers_.indexOf(observer)) {
              this.observers_.push(observer);
          }
          if (!this.connected_) {
              this.connect_();
          }
      };
      ResizeObserverController.prototype.removeObserver = function (observer) {
          var observers = this.observers_;
          var index = observers.indexOf(observer);
          if (~index) {
              observers.splice(index, 1);
          }
          if (!observers.length && this.connected_) {
              this.disconnect_();
          }
      };
      ResizeObserverController.prototype.refresh = function () {
          var changesDetected = this.updateObservers_();
          // be future ones caused by CSS transitions.
          if (changesDetected) {
              this.refresh();
          }
      };
      ResizeObserverController.prototype.updateObservers_ = function () {
          var activeObservers = this.observers_.filter(function (observer) {
              return observer.gatherActive(), observer.hasActive();
          });
          
          activeObservers.forEach(function (observer) { return observer.broadcastActive(); });
          return activeObservers.length > 0;
      };
      ResizeObserverController.prototype.connect_ = function () {
          // have been already added.
          if (!isBrowser || this.connected_) {
              return;
          }
          // delayed transitions. This way it's possible to capture at least the
          document.addEventListener('transitionend', this.onTransitionEnd_);
          window.addEventListener('resize', this.refresh);
          if (mutationObserverSupported) {
              this.mutationsObserver_ = new MutationObserver(this.refresh);
              this.mutationsObserver_.observe(document, {
                  attributes: true,
                  childList: true,
                  characterData: true,
                  subtree: true
              });
          }
          else {
              document.addEventListener('DOMSubtreeModified', this.refresh);
              this.mutationEventsAdded_ = true;
          }
          this.connected_ = true;
      };
      ResizeObserverController.prototype.disconnect_ = function () {
          // have been already removed.
          if (!isBrowser || !this.connected_) {
              return;
          }
          document.removeEventListener('transitionend', this.onTransitionEnd_);
          window.removeEventListener('resize', this.refresh);
          if (this.mutationsObserver_) {
              this.mutationsObserver_.disconnect();
          }
          if (this.mutationEventsAdded_) {
              document.removeEventListener('DOMSubtreeModified', this.refresh);
          }
          this.mutationsObserver_ = null;
          this.mutationEventsAdded_ = false;
          this.connected_ = false;
      };
      ResizeObserverController.prototype.onTransitionEnd_ = function (_a) {
          var _b = _a.propertyName, propertyName = _b === void 0 ? '' : _b;
          var isReflowProperty = transitionKeys.some(function (key) {
              return !!~propertyName.indexOf(key);
          });
          if (isReflowProperty) {
              this.refresh();
          }
      };
      ResizeObserverController.getInstance = function () {
          if (!this.instance_) {
              this.instance_ = new ResizeObserverController();
          }
          return this.instance_;
      };
      ResizeObserverController.instance_ = null;
      return ResizeObserverController;
  }());
  var defineConfigurable = (function (target, props) {
      for (var _i = 0, _a = Object.keys(props); _i < _a.length; _i++) {
          var key = _a[_i];
          Object.defineProperty(target, key, {
              value: props[key],
              enumerable: false,
              writable: false,
              configurable: true
          });
      }
      return target;
  });
  var getWindowOf = (function (target) {
      var ownerGlobal = target && target.ownerDocument && target.ownerDocument.defaultView;
      return ownerGlobal || global$1$1;
  });
  var emptyRect = createRectInit(0, 0, 0, 0);
  function toFloat(value) {
      return parseFloat(value) || 0;
  }
  function getBordersSize(styles) {
      var positions = [];
      for (var _i = 1; _i < arguments.length; _i++) {
          positions[_i - 1] = arguments[_i];
      }
      return positions.reduce(function (size, position) {
          var value = styles['border-' + position + '-width'];
          return size + toFloat(value);
      }, 0);
  }
  function getPaddings(styles) {
      var positions = ['top', 'right', 'bottom', 'left'];
      var paddings = {};
      for (var _i = 0, positions_1 = positions; _i < positions_1.length; _i++) {
          var position = positions_1[_i];
          var value = styles['padding-' + position];
          paddings[position] = toFloat(value);
      }
      return paddings;
  }
  function getSVGContentRect(target) {
      var bbox = target.getBBox();
      return createRectInit(0, 0, bbox.width, bbox.height);
  }
  function getHTMLElementContentRect(target) {
      
      var clientWidth = target.clientWidth, clientHeight = target.clientHeight;
      
      if (!clientWidth && !clientHeight) {
          return emptyRect;
      }
      var styles = getWindowOf(target).getComputedStyle(target);
      var paddings = getPaddings(styles);
      var horizPad = paddings.left + paddings.right;
      var vertPad = paddings.top + paddings.bottom;
      
      var width = toFloat(styles.width), height = toFloat(styles.height);
      
      if (styles.boxSizing === 'border-box') {
          
          if (Math.round(width + horizPad) !== clientWidth) {
              width -= getBordersSize(styles, 'left', 'right') + horizPad;
          }
          if (Math.round(height + vertPad) !== clientHeight) {
              height -= getBordersSize(styles, 'top', 'bottom') + vertPad;
          }
      }
      
      if (!isDocumentElement(target)) {
          
          var vertScrollbar = Math.round(width + horizPad) - clientWidth;
          var horizScrollbar = Math.round(height + vertPad) - clientHeight;
          
          if (Math.abs(vertScrollbar) !== 1) {
              width -= vertScrollbar;
          }
          if (Math.abs(horizScrollbar) !== 1) {
              height -= horizScrollbar;
          }
      }
      return createRectInit(paddings.left, paddings.top, width, height);
  }
  var isSVGGraphicsElement = (function () {
      // interface.
      if (typeof SVGGraphicsElement !== 'undefined') {
          return function (target) { return target instanceof getWindowOf(target).SVGGraphicsElement; };
      }
      
      return function (target) { return (target instanceof getWindowOf(target).SVGElement &&
          typeof target.getBBox === 'function'); };
  })();
  function isDocumentElement(target) {
      return target === getWindowOf(target).document.documentElement;
  }
  function getContentRect(target) {
      if (!isBrowser) {
          return emptyRect;
      }
      if (isSVGGraphicsElement(target)) {
          return getSVGContentRect(target);
      }
      return getHTMLElementContentRect(target);
  }
  function createReadOnlyRect(_a) {
      var x = _a.x, y = _a.y, width = _a.width, height = _a.height;
      var Constr = typeof DOMRectReadOnly !== 'undefined' ? DOMRectReadOnly : Object;
      var rect = Object.create(Constr.prototype);
      defineConfigurable(rect, {
          x: x, y: y, width: width, height: height,
          top: y,
          right: x + width,
          bottom: height + y,
          left: x
      });
      return rect;
  }
  function createRectInit(x, y, width, height) {
      return { x: x, y: y, width: width, height: height };
  }
  var ResizeObservation = /** @class */ (function () {
      function ResizeObservation(target) {
          this.broadcastWidth = 0;
          this.broadcastHeight = 0;
          this.contentRect_ = createRectInit(0, 0, 0, 0);
          this.target = target;
      }
      ResizeObservation.prototype.isActive = function () {
          var rect = getContentRect(this.target);
          this.contentRect_ = rect;
          return (rect.width !== this.broadcastWidth ||
              rect.height !== this.broadcastHeight);
      };
      ResizeObservation.prototype.broadcastRect = function () {
          var rect = this.contentRect_;
          this.broadcastWidth = rect.width;
          this.broadcastHeight = rect.height;
          return rect;
      };
      return ResizeObservation;
  }());

  var ResizeObserverEntry = /** @class */ (function () {
      function ResizeObserverEntry(target, rectInit) {
          var contentRect = createReadOnlyRect(rectInit);
          
          defineConfigurable(this, { target: target, contentRect: contentRect });
      }
      return ResizeObserverEntry;
  }());

  var ResizeObserverSPI = /** @class */ (function () {
      function ResizeObserverSPI(callback, controller, callbackCtx) {
          this.activeObservations_ = [];
          this.observations_ = new MapShim();
          if (typeof callback !== 'function') {
              throw new TypeError('The callback provided as parameter 1 is not a function.');
          }
          this.callback_ = callback;
          this.controller_ = controller;
          this.callbackCtx_ = callbackCtx;
      }
      ResizeObserverSPI.prototype.observe = function (target) {
          if (!arguments.length) {
              throw new TypeError('1 argument required, but only 0 present.');
          }
          if (typeof Element === 'undefined' || !(Element instanceof Object)) {
              return;
          }
          if (!(target instanceof getWindowOf(target).Element)) {
              throw new TypeError('parameter 1 is not of type "Element".');
          }
          var observations = this.observations_;
          if (observations.has(target)) {
              return;
          }
          observations.set(target, new ResizeObservation(target));
          this.controller_.addObserver(this);
          this.controller_.refresh();
      };
      ResizeObserverSPI.prototype.unobserve = function (target) {
          if (!arguments.length) {
              throw new TypeError('1 argument required, but only 0 present.');
          }
          if (typeof Element === 'undefined' || !(Element instanceof Object)) {
              return;
          }
          if (!(target instanceof getWindowOf(target).Element)) {
              throw new TypeError('parameter 1 is not of type "Element".');
          }
          var observations = this.observations_;
          if (!observations.has(target)) {
              return;
          }
          observations.delete(target);
          if (!observations.size) {
              this.controller_.removeObserver(this);
          }
      };
      ResizeObserverSPI.prototype.disconnect = function () {
          this.clearActive();
          this.observations_.clear();
          this.controller_.removeObserver(this);
      };
      ResizeObserverSPI.prototype.gatherActive = function () {
          var _this = this;
          this.clearActive();
          this.observations_.forEach(function (observation) {
              if (observation.isActive()) {
                  _this.activeObservations_.push(observation);
              }
          });
      };
      ResizeObserverSPI.prototype.broadcastActive = function () {
          if (!this.hasActive()) {
              return;
          }
          var ctx = this.callbackCtx_;
          var entries = this.activeObservations_.map(function (observation) {
              return new ResizeObserverEntry(observation.target, observation.broadcastRect());
          });
          this.callback_.call(ctx, entries, ctx);
          this.clearActive();
      };
      ResizeObserverSPI.prototype.clearActive = function () {
          this.activeObservations_.splice(0);
      };
      ResizeObserverSPI.prototype.hasActive = function () {
          return this.activeObservations_.length > 0;
      };
      return ResizeObserverSPI;
  }());
  
  var observers = typeof WeakMap !== 'undefined' ? new WeakMap() : new MapShim();
  var ResizeObserver = /** @class */ (function () {
      function ResizeObserver(callback) {
          if (!(this instanceof ResizeObserver)) {
              throw new TypeError('Cannot call a class as a function.');
          }
          if (!arguments.length) {
              throw new TypeError('1 argument required, but only 0 present.');
          }
          var controller = ResizeObserverController.getInstance();
          var observer = new ResizeObserverSPI(callback, controller, this);
          observers.set(this, observer);
      }
      return ResizeObserver;
  }());
  [
      'observe',
      'unobserve',
      'disconnect'
  ].forEach(function (method) {
      ResizeObserver.prototype[method] = function () {
          var _a;
          return (_a = observers.get(this))[method].apply(_a, arguments);
      };
  });

  var index = (function () {
      if (typeof global$1$1.ResizeObserver !== 'undefined') {
          return global$1$1.ResizeObserver;
      }
      return ResizeObserver;
  })();

  var canUseDOM = !!(
    typeof window !== 'undefined' &&
    window.document &&
    window.document.createElement
  );

  var canUseDom = canUseDOM;

  function scrollbarWidth() {
    if (typeof document === 'undefined') {
      return 0;
    }

    var body = document.body;
    var box = document.createElement('div');
    var boxStyle = box.style;
    boxStyle.position = 'fixed';
    boxStyle.left = 0;
    boxStyle.visibility = 'hidden';
    boxStyle.overflowY = 'scroll';
    body.appendChild(box);
    var width = box.getBoundingClientRect().right;
    body.removeChild(box);
    return width;
  }

  var VesuvioBar =
  function () {
    function VesuvioBar(element, options) {
      var _this = this;

      this.onScroll = function () {
        if (!_this.scrollXTicking) {
          window.requestAnimationFrame(_this.scrollX);
          _this.scrollXTicking = true;
        }

        if (!_this.scrollYTicking) {
          window.requestAnimationFrame(_this.scrollY);
          _this.scrollYTicking = true;
        }
      };

      this.scrollX = function () {
        if (_this.axis.x.isOverflowing) {
          _this.showScrollbar('x');

          _this.positionScrollbar('x');
        }

        _this.scrollXTicking = false;
      };

      this.scrollY = function () {
        if (_this.axis.y.isOverflowing) {
          _this.showScrollbar('y');

          _this.positionScrollbar('y');
        }

        _this.scrollYTicking = false;
      };

      this.onMouseEnter = function () {
        _this.showScrollbar('x');

        _this.showScrollbar('y');
      };

      this.onMouseMove = function (e) {
        _this.mouseX = e.clientX;
        _this.mouseY = e.clientY;

        if (_this.axis.x.isOverflowing || _this.axis.x.forceVisible) {
          _this.onMouseMoveForAxis('x');
        }

        if (_this.axis.y.isOverflowing || _this.axis.y.forceVisible) {
          _this.onMouseMoveForAxis('y');
        }
      };

      this.onMouseLeave = function () {
        _this.onMouseMove.cancel();

        if (_this.axis.x.isOverflowing || _this.axis.x.forceVisible) {
          _this.onMouseLeaveForAxis('x');
        }

        if (_this.axis.y.isOverflowing || _this.axis.y.forceVisible) {
          _this.onMouseLeaveForAxis('y');
        }

        _this.mouseX = -1;
        _this.mouseY = -1;
      };

      this.onWindowResize = function () {
        _this.scrollbarWidth = scrollbarWidth();

        _this.hideNativeScrollbar();
      };

      this.hideScrollbars = function () {
        _this.axis.x.track.rect = _this.axis.x.track.el.getBoundingClientRect();
        _this.axis.y.track.rect = _this.axis.y.track.el.getBoundingClientRect();

        if (!_this.isWithinBounds(_this.axis.y.track.rect)) {
          _this.axis.y.scrollbar.el.classList.remove(_this.classNames.visible);

          _this.axis.y.isVisible = false;
        }

        if (!_this.isWithinBounds(_this.axis.x.track.rect)) {
          _this.axis.x.scrollbar.el.classList.remove(_this.classNames.visible);

          _this.axis.x.isVisible = false;
        }
      };

      this.onPointerEvent = function (e) {
        var isWithinBoundsY, isWithinBoundsX;
        _this.axis.x.scrollbar.rect = _this.axis.x.scrollbar.el.getBoundingClientRect();
        _this.axis.y.scrollbar.rect = _this.axis.y.scrollbar.el.getBoundingClientRect();

        if (_this.axis.x.isOverflowing || _this.axis.x.forceVisible) {
          isWithinBoundsX = _this.isWithinBounds(_this.axis.x.scrollbar.rect);
        }

        if (_this.axis.y.isOverflowing || _this.axis.y.forceVisible) {
          isWithinBoundsY = _this.isWithinBounds(_this.axis.y.scrollbar.rect);
        } 

        if (isWithinBoundsY || isWithinBoundsX) {
          
          e.preventDefault(); 

          e.stopPropagation();

          if (e.type === 'mousedown') {
            if (isWithinBoundsY) {
              _this.onDragStart(e, 'y');
            }

            if (isWithinBoundsX) {
              _this.onDragStart(e, 'x');
            }
          }
        }
      };

      this.drag = function (e) {
        var eventOffset;
        var track = _this.axis[_this.draggedAxis].track;
        var trackSize = track.rect[_this.axis[_this.draggedAxis].sizeAttr];
        var scrollbar = _this.axis[_this.draggedAxis].scrollbar;
        e.preventDefault();
        e.stopPropagation();

        if (_this.draggedAxis === 'y') {
          eventOffset = e.pageY;
        } else {
          eventOffset = e.pageX;
        }


        var dragPos = eventOffset - track.rect[_this.axis[_this.draggedAxis].offsetAttr] - _this.axis[_this.draggedAxis].dragOffset;

        var dragPerc = dragPos / track.rect[_this.axis[_this.draggedAxis].sizeAttr];

        var scrollPos = dragPerc * _this.contentWrapperEl[_this.axis[_this.draggedAxis].scrollSizeAttr];

        if (_this.draggedAxis === 'x') {
          scrollPos = _this.isRtl && VesuvioBar.getRtlHelpers().isRtlScrollbarInverted ? scrollPos - (trackSize + scrollbar.size) : scrollPos;
          scrollPos = _this.isRtl && VesuvioBar.getRtlHelpers().isRtlScrollingInverted ? -scrollPos : scrollPos;
        }

        _this.contentWrapperEl[_this.axis[_this.draggedAxis].scrollOffsetAttr] = scrollPos;
      };

      this.onEndDrag = function (e) {
        e.preventDefault();
        e.stopPropagation();

        _this.el.classList.remove(_this.classNames.dragging);

        document.removeEventListener('mousemove', _this.drag, true);
        document.removeEventListener('mouseup', _this.onEndDrag, true);
        _this.removePreventClickId = window.setTimeout(function () {
          // generated simultaneously with mouseup.
          document.removeEventListener('click', _this.preventClick, true);
          document.removeEventListener('dblclick', _this.preventClick, true);
          _this.removePreventClickId = null;
        });
      };

      this.preventClick = function (e) {
        e.preventDefault();
        e.stopPropagation();
      };

      this.el = element;
      this.flashTimeout;
      this.contentEl;
      this.contentWrapperEl;
      this.offsetEl;
      this.maskEl;
      this.globalObserver;
      this.mutationObserver;
      this.resizeObserver;
      this.scrollbarWidth;
      this.minScrollbarWidth = 20;
      this.options = Object.assign({}, VesuvioBar.defaultOptions, options);
      this.classNames = Object.assign({}, VesuvioBar.defaultOptions.classNames, this.options.classNames);
      this.isRtl;
      this.axis = {
        x: {
          scrollOffsetAttr: 'scrollLeft',
          sizeAttr: 'width',
          scrollSizeAttr: 'scrollWidth',
          offsetAttr: 'left',
          overflowAttr: 'overflowX',
          dragOffset: 0,
          isOverflowing: true,
          isVisible: false,
          forceVisible: false,
          track: {},
          scrollbar: {}
        },
        y: {
          scrollOffsetAttr: 'scrollTop',
          sizeAttr: 'height',
          scrollSizeAttr: 'scrollHeight',
          offsetAttr: 'top',
          overflowAttr: 'overflowY',
          dragOffset: 0,
          isOverflowing: true,
          isVisible: false,
          forceVisible: false,
          track: {},
          scrollbar: {}
        }
      };
      this.removePreventClickId = null; 

      if (VesuvioBar.instances.has(this.el)) {
        return;
      }

      this.recalculate = lodash_throttle(this.recalculate.bind(this), 64);
      this.onMouseMove = lodash_throttle(this.onMouseMove.bind(this), 64);
      this.hideScrollbars = lodash_debounce(this.hideScrollbars.bind(this), this.options.timeout);
      this.onWindowResize = lodash_debounce(this.onWindowResize.bind(this), 64, {
        leading: true
      });
      VesuvioBar.getRtlHelpers = lodash_memoize(VesuvioBar.getRtlHelpers);
      this.init();
    }
    


    VesuvioBar.getRtlHelpers = function getRtlHelpers() {
      var dummyDiv = document.createElement('div');
      dummyDiv.innerHTML = '<div class="hs-dummy-scrollbar-size"><div style="height: 200%; width: 200%; margin: 10px 0;"></div></div>';
      var scrollbarDummyEl = dummyDiv.firstElementChild;
      document.body.appendChild(scrollbarDummyEl);
      var dummyContainerChild = scrollbarDummyEl.firstElementChild;
      scrollbarDummyEl.scrollLeft = 0;
      var dummyContainerOffset = VesuvioBar.getOffset(scrollbarDummyEl);
      var dummyContainerChildOffset = VesuvioBar.getOffset(dummyContainerChild);
      scrollbarDummyEl.scrollLeft = 999;
      var dummyContainerScrollOffsetAfterScroll = VesuvioBar.getOffset(dummyContainerChild);
      return {
        isRtlScrollingInverted: dummyContainerOffset.left !== dummyContainerChildOffset.left && dummyContainerChildOffset.left - dummyContainerScrollOffsetAfterScroll.left !== 0,
        isRtlScrollbarInverted: dummyContainerOffset.left !== dummyContainerChildOffset.left
      };
    };

    VesuvioBar.initHtmlApi = function initHtmlApi() {
      this.initDOMLoadedElements = this.initDOMLoadedElements.bind(this); // MutationObserver is IE11+

      if (typeof MutationObserver !== 'undefined') {
        this.globalObserver = new MutationObserver(function (mutations) {
          mutations.forEach(function (mutation) {
            Array.prototype.forEach.call(mutation.addedNodes, function (addedNode) {
              if (addedNode.nodeType === 1) {
                if (addedNode.hasAttribute('data-vlscrollbar')) {
                  !VesuvioBar.instances.has(addedNode) && new VesuvioBar(addedNode, VesuvioBar.getElOptions(addedNode));
                } else {
                  Array.prototype.forEach.call(addedNode.querySelectorAll('[data-vlscrollbar]'), function (el) {
                    !VesuvioBar.instances.has(el) && new VesuvioBar(el, VesuvioBar.getElOptions(el));
                  });
                }
              }
            });
            Array.prototype.forEach.call(mutation.removedNodes, function (removedNode) {
              if (removedNode.nodeType === 1) {
                if (removedNode.hasAttribute('data-vlscrollbar')) {
                  VesuvioBar.instances.has(removedNode) && VesuvioBar.instances.get(removedNode).unMount();
                } else {
                  Array.prototype.forEach.call(removedNode.querySelectorAll('[data-vlscrollbar]'), function (el) {
                    VesuvioBar.instances.has(el) && VesuvioBar.instances.get(el).unMount();
                  });
                }
              }
            });
          });
        });
        this.globalObserver.observe(document, {
          childList: true,
          subtree: true
        });
      }


      if (document.readyState === 'complete' || document.readyState !== 'loading' && !document.documentElement.doScroll) {
        window.setTimeout(this.initDOMLoadedElements);
      } else {
        document.addEventListener('DOMContentLoaded', this.initDOMLoadedElements);
        window.addEventListener('load', this.initDOMLoadedElements);
      }
    };

    VesuvioBar.getElOptions = function getElOptions(el) {
      var options = Array.prototype.reduce.call(el.attributes, function (acc, attribute) {
        var option = attribute.name.match(/data-vlscrollbar-(.+)/);

        if (option) {
          var key = option[1].replace(/\W+(.)/g, function (x, chr) {
            return chr.toUpperCase();
          });

          switch (attribute.value) {
            case 'true':
              acc[key] = true;
              break;

            case 'false':
              acc[key] = false;
              break;

            case undefined:
              acc[key] = true;
              break;

            default:
              acc[key] = attribute.value;
          }
        }

        return acc;
      }, {});
      return options;
    };

    VesuvioBar.removeObserver = function removeObserver() {
      this.globalObserver.disconnect();
    };

    VesuvioBar.initDOMLoadedElements = function initDOMLoadedElements() {
      document.removeEventListener('DOMContentLoaded', this.initDOMLoadedElements);
      window.removeEventListener('load', this.initDOMLoadedElements);
      Array.prototype.forEach.call(document.querySelectorAll('[data-simplebar]'), function (el) {
        if (!VesuvioBar.instances.has(el)) new VesuvioBar(el, VesuvioBar.getElOptions(el));
      });
    };

    VesuvioBar.getOffset = function getOffset(el) {
      var rect = el.getBoundingClientRect();
      return {
        top: rect.top + (window.pageYOffset || document.documentElement.scrollTop),
        left: rect.left + (window.pageXOffset || document.documentElement.scrollLeft)
      };
    };

    var _proto = VesuvioBar.prototype;

    _proto.init = function init() {
      VesuvioBar.instances.set(this.el, this); 

      if (canUseDom) {
        this.initDOM();
        this.scrollbarWidth = scrollbarWidth();
        this.recalculate();
        this.initListeners();
      }
    };

    _proto.initDOM = function initDOM() {
      var _this2 = this;
      if (Array.prototype.filter.call(this.el.children, function (child) {
        return child.classList.contains(_this2.classNames.wrapper);
      }).length) {
        this.wrapperEl = this.el.querySelector("." + this.classNames.wrapper);
        this.contentWrapperEl = this.options.scrollableNode || this.el.querySelector("." + this.classNames.contentWrapper);
        this.contentEl = this.options.contentNode || this.el.querySelector("." + this.classNames.contentEl);
        this.offsetEl = this.el.querySelector("." + this.classNames.offset);
        this.maskEl = this.el.querySelector("." + this.classNames.mask);
        this.placeholderEl = this.el.querySelector("." + this.classNames.placeholder);
        this.heightAutoObserverWrapperEl = this.el.querySelector("." + this.classNames.heightAutoObserverWrapperEl);
        this.heightAutoObserverEl = this.el.querySelector("." + this.classNames.heightAutoObserverEl);
        this.axis.x.track.el = this.el.querySelector("." + this.classNames.track + "." + this.classNames.horizontal);
        this.axis.y.track.el = this.el.querySelector("." + this.classNames.track + "." + this.classNames.vertical);
      } else {
        this.wrapperEl = document.createElement('div');
        this.contentWrapperEl = document.createElement('div');
        this.offsetEl = document.createElement('div');
        this.maskEl = document.createElement('div');
        this.contentEl = document.createElement('div');
        this.placeholderEl = document.createElement('div');
        this.heightAutoObserverWrapperEl = document.createElement('div');
        this.heightAutoObserverEl = document.createElement('div');
        this.wrapperEl.classList.add(this.classNames.wrapper);
        this.contentWrapperEl.classList.add(this.classNames.contentWrapper);
        this.offsetEl.classList.add(this.classNames.offset);
        this.maskEl.classList.add(this.classNames.mask);
        this.contentEl.classList.add(this.classNames.contentEl);
        this.placeholderEl.classList.add(this.classNames.placeholder);
        this.heightAutoObserverWrapperEl.classList.add(this.classNames.heightAutoObserverWrapperEl);
        this.heightAutoObserverEl.classList.add(this.classNames.heightAutoObserverEl);

        while (this.el.firstChild) {
          this.contentEl.appendChild(this.el.firstChild);
        }

        this.contentWrapperEl.appendChild(this.contentEl);
        this.offsetEl.appendChild(this.contentWrapperEl);
        this.maskEl.appendChild(this.offsetEl);
        this.heightAutoObserverWrapperEl.appendChild(this.heightAutoObserverEl);
        this.wrapperEl.appendChild(this.heightAutoObserverWrapperEl);
        this.wrapperEl.appendChild(this.maskEl);
        this.wrapperEl.appendChild(this.placeholderEl);
        this.el.appendChild(this.wrapperEl);
      }

      if (!this.axis.x.track.el || !this.axis.y.track.el) {
        var track = document.createElement('div');
        var scrollbar = document.createElement('div');
        track.classList.add(this.classNames.track);
        scrollbar.classList.add(this.classNames.scrollbar);
        track.appendChild(scrollbar);
        this.axis.x.track.el = track.cloneNode(true);
        this.axis.x.track.el.classList.add(this.classNames.horizontal);
        this.axis.y.track.el = track.cloneNode(true);
        this.axis.y.track.el.classList.add(this.classNames.vertical);
        this.el.appendChild(this.axis.x.track.el);
        this.el.appendChild(this.axis.y.track.el);
      }

      this.axis.x.scrollbar.el = this.axis.x.track.el.querySelector("." + this.classNames.scrollbar);
      this.axis.y.scrollbar.el = this.axis.y.track.el.querySelector("." + this.classNames.scrollbar);

      if (!this.options.autoHide) {
        this.axis.x.scrollbar.el.classList.add(this.classNames.visible);
        this.axis.y.scrollbar.el.classList.add(this.classNames.visible);
      }

      this.el.setAttribute('data-simplebar', 'init');
    };

    _proto.initListeners = function initListeners() {
      var _this3 = this;
      if (this.options.autoHide) {
        this.el.addEventListener('mouseenter', this.onMouseEnter);
      }

      ['mousedown', 'click', 'dblclick'].forEach(function (e) {
        _this3.el.addEventListener(e, _this3.onPointerEvent, true);
      });
      ['touchstart', 'touchend', 'touchmove'].forEach(function (e) {
        _this3.el.addEventListener(e, _this3.onPointerEvent, {
          capture: true,
          passive: true
        });
      });
      this.el.addEventListener('mousemove', this.onMouseMove);
      this.el.addEventListener('mouseleave', this.onMouseLeave);
      this.contentWrapperEl.addEventListener('scroll', this.onScroll); 

      window.addEventListener('resize', this.onWindowResize);
      this.resizeObserver = new index(this.recalculate);
      this.resizeObserver.observe(this.el);
      this.resizeObserver.observe(this.contentEl);
    };

    _proto.recalculate = function recalculate() {
      var isHeightAuto = this.heightAutoObserverEl.offsetHeight <= 1;
      var isWidthAuto = this.heightAutoObserverEl.offsetWidth <= 1;
      this.elStyles = window.getComputedStyle(this.el);
      this.isRtl = this.elStyles.direction === 'rtl';
      this.contentEl.style.padding = this.elStyles.paddingTop + " " + this.elStyles.paddingRight + " " + this.elStyles.paddingBottom + " " + this.elStyles.paddingLeft;
      this.wrapperEl.style.margin = "-" + this.elStyles.paddingTop + " -" + this.elStyles.paddingRight + " -" + this.elStyles.paddingBottom + " -" + this.elStyles.paddingLeft;
      this.contentWrapperEl.style.height = isHeightAuto ? 'auto' : '100%'; 

      this.placeholderEl.style.width = isWidthAuto ? this.contentEl.offsetWidth + "px" : 'auto';
      this.placeholderEl.style.height = this.contentEl.scrollHeight + "px"; 

      this.axis.x.isOverflowing = this.contentWrapperEl.scrollWidth > this.contentWrapperEl.offsetWidth;
      this.axis.y.isOverflowing = this.contentWrapperEl.scrollHeight > this.contentWrapperEl.offsetHeight; 

      this.axis.x.isOverflowing = this.elStyles.overflowX === 'hidden' ? false : this.axis.x.isOverflowing;
      this.axis.y.isOverflowing = this.elStyles.overflowY === 'hidden' ? false : this.axis.y.isOverflowing;
      this.axis.x.forceVisible = this.options.forceVisible === 'x' || this.options.forceVisible === true;
      this.axis.y.forceVisible = this.options.forceVisible === 'y' || this.options.forceVisible === true;
      this.hideNativeScrollbar();
      this.axis.x.track.rect = this.axis.x.track.el.getBoundingClientRect();
      this.axis.y.track.rect = this.axis.y.track.el.getBoundingClientRect();
      this.axis.x.scrollbar.size = this.getScrollbarSize('x');
      this.axis.y.scrollbar.size = this.getScrollbarSize('y');
      this.axis.x.scrollbar.el.style.width = this.axis.x.scrollbar.size + "px";
      this.axis.y.scrollbar.el.style.height = this.axis.y.scrollbar.size + "px";
      this.positionScrollbar('x');
      this.positionScrollbar('y');
      this.toggleTrackVisibility('x');
      this.toggleTrackVisibility('y');
    }
    ;

    _proto.getScrollbarSize = function getScrollbarSize(axis) {
      if (axis === void 0) {
        axis = 'y';
      }

      var contentSize = this.scrollbarWidth ? this.contentWrapperEl[this.axis[axis].scrollSizeAttr] : this.contentWrapperEl[this.axis[axis].scrollSizeAttr] - this.minScrollbarWidth;
      var trackSize = this.axis[axis].track.rect[this.axis[axis].sizeAttr];
      var scrollbarSize;

      if (!this.axis[axis].isOverflowing) {
        return;
      }

      var scrollbarRatio = trackSize / contentSize; // Calculate new height/position of drag handle.

      scrollbarSize = Math.max(~~(scrollbarRatio * trackSize), this.options.scrollbarMinSize);

      if (this.options.scrollbarMaxSize) {
        scrollbarSize = Math.min(scrollbarSize, this.options.scrollbarMaxSize);
      }

      return scrollbarSize;
    };

    _proto.positionScrollbar = function positionScrollbar(axis) {
      if (axis === void 0) {
        axis = 'y';
      }

      var contentSize = this.contentWrapperEl[this.axis[axis].scrollSizeAttr];
      var trackSize = this.axis[axis].track.rect[this.axis[axis].sizeAttr];
      var hostSize = parseInt(this.elStyles[this.axis[axis].sizeAttr], 10);
      var scrollbar = this.axis[axis].scrollbar;
      var scrollOffset = this.contentWrapperEl[this.axis[axis].scrollOffsetAttr];
      scrollOffset = axis === 'x' && this.isRtl && VesuvioBar.getRtlHelpers().isRtlScrollingInverted ? -scrollOffset : scrollOffset;
      var scrollPourcent = scrollOffset / (contentSize - hostSize);
      var handleOffset = ~~((trackSize - scrollbar.size) * scrollPourcent);
      handleOffset = axis === 'x' && this.isRtl && VesuvioBar.getRtlHelpers().isRtlScrollbarInverted ? handleOffset + (trackSize - scrollbar.size) : handleOffset;
        scrollbar.el.style.transform = axis === 'x' ? "translate3d(" + handleOffset + "px, 0, 0)" : "translate3d(0, " + handleOffset + "px, 0)";

        scrollbar.el.style.visibility = 'visible';
        UpdateWF({"val": scrollOffset ,"axis":axis});
        //    scrollbar.el.setAttribute('data-setting', '{"val":"' + handleOffset + '","axis":"' + axis + '"}');
        
    };

    _proto.toggleTrackVisibility = function toggleTrackVisibility(axis) {
      if (axis === void 0) {
        axis = 'y';
      }

      var track = this.axis[axis].track.el;
      var scrollbar = this.axis[axis].scrollbar.el;

      if (this.axis[axis].isOverflowing || this.axis[axis].forceVisible) {
        track.style.visibility = 'visible';
        this.contentWrapperEl.style[this.axis[axis].overflowAttr] = 'scroll';
      } else {
        track.style.visibility = 'hidden';
        this.contentWrapperEl.style[this.axis[axis].overflowAttr] = 'hidden';
      }


      if (this.axis[axis].isOverflowing) {
        scrollbar.style.display = 'block';
      } else {
        scrollbar.style.display = 'none';
      }
    };

    _proto.hideNativeScrollbar = function hideNativeScrollbar() {
      this.offsetEl.style[this.isRtl ? 'left' : 'right'] = this.axis.y.isOverflowing || this.axis.y.forceVisible ? "-" + (this.scrollbarWidth || this.minScrollbarWidth) + "px" : 0;
      this.offsetEl.style.bottom = this.axis.x.isOverflowing || this.axis.x.forceVisible ? "-" + (this.scrollbarWidth || this.minScrollbarWidth) + "px" : 0; // If floating scrollbar

      if (!this.scrollbarWidth) {
        var paddingDirection = [this.isRtl ? 'paddingLeft' : 'paddingRight'];
        this.contentWrapperEl.style[paddingDirection] = this.axis.y.isOverflowing || this.axis.y.forceVisible ? this.minScrollbarWidth + "px" : 0;
        this.contentWrapperEl.style.paddingBottom = this.axis.x.isOverflowing || this.axis.x.forceVisible ? this.minScrollbarWidth + "px" : 0;
      }
    }
    ;

    _proto.onMouseMoveForAxis = function onMouseMoveForAxis(axis) {
      if (axis === void 0) {
        axis = 'y';
      }

      this.axis[axis].track.rect = this.axis[axis].track.el.getBoundingClientRect();
      this.axis[axis].scrollbar.rect = this.axis[axis].scrollbar.el.getBoundingClientRect();
      var isWithinScrollbarBoundsX = this.isWithinBounds(this.axis[axis].scrollbar.rect);

      if (isWithinScrollbarBoundsX) {
        this.axis[axis].scrollbar.el.classList.add(this.classNames.hover);
      } else {
        this.axis[axis].scrollbar.el.classList.remove(this.classNames.hover);
      }

      if (this.isWithinBounds(this.axis[axis].track.rect)) {
        this.showScrollbar(axis);
        this.axis[axis].track.el.classList.add(this.classNames.hover);
      } else {
        this.axis[axis].track.el.classList.remove(this.classNames.hover);
      }
    };

    _proto.onMouseLeaveForAxis = function onMouseLeaveForAxis(axis) {
      if (axis === void 0) {
        axis = 'y';
      }

      this.axis[axis].track.el.classList.remove(this.classNames.hover);
      this.axis[axis].scrollbar.el.classList.remove(this.classNames.hover);
    };
    _proto.showScrollbar = function showScrollbar(axis) {
      if (axis === void 0) {
        axis = 'y';
      }

      var scrollbar = this.axis[axis].scrollbar.el;

      if (!this.axis[axis].isVisible) {
        scrollbar.classList.add(this.classNames.visible);
        this.axis[axis].isVisible = true;
      }

      if (this.options.autoHide) {
        this.hideScrollbars();
      }
    }
    ;
    _proto.onDragStart = function onDragStart(e, axis) {
      if (axis === void 0) {
        axis = 'y';
      }

      var scrollbar = this.axis[axis].scrollbar.el; // Measure how far the user's mouse is from the top of the scrollbar drag handle.

      var eventOffset = axis === 'y' ? e.pageY : e.pageX;
      this.axis[axis].dragOffset = eventOffset - scrollbar.getBoundingClientRect()[this.axis[axis].offsetAttr];
      this.draggedAxis = axis;
      this.el.classList.add(this.classNames.dragging);
      document.addEventListener('mousemove', this.drag, true);
      document.addEventListener('mouseup', this.onEndDrag, true);

      if (this.removePreventClickId === null) {
        document.addEventListener('click', this.preventClick, true);
        document.addEventListener('dblclick', this.preventClick, true);
      } else {
        window.clearTimeout(this.removePreventClickId);
        this.removePreventClickId = null;
      }
    }
    ;
    _proto.getContentElement = function getContentElement() {
      return this.contentEl;
    }
    ;

    _proto.getScrollElement = function getScrollElement() {
      return this.contentWrapperEl;
    };

    _proto.removeListeners = function removeListeners() {
      var _this4 = this;
      if (this.options.autoHide) {
        this.el.removeEventListener('mouseenter', this.onMouseEnter);
      }

      ['mousedown', 'click', 'dblclick'].forEach(function (e) {
        _this4.el.removeEventListener(e, _this4.onPointerEvent, true);
      });
      ['touchstart', 'touchend', 'touchmove'].forEach(function (e) {
        _this4.el.removeEventListener(e, _this4.onPointerEvent, {
          capture: true,
          passive: true
        });
      });
      this.el.removeEventListener('mousemove', this.onMouseMove);
      this.el.removeEventListener('mouseleave', this.onMouseLeave);
      this.contentWrapperEl.removeEventListener('scroll', this.onScroll);
      window.removeEventListener('resize', this.onWindowResize);
      this.mutationObserver && this.mutationObserver.disconnect();
      this.resizeObserver.disconnect(); // Cancel all debounced functions

      this.recalculate.cancel();
      this.onMouseMove.cancel();
      this.hideScrollbars.cancel();
      this.onWindowResize.cancel();
    }
    ;

    _proto.unMount = function unMount() {
      this.removeListeners();
      VesuvioBar.instances.delete(this.el);
    }
    ;

    _proto.isChildNode = function isChildNode(el) {
      if (el === null) return false;
      if (el === this.el) return true;
      return this.isChildNode(el.parentNode);
    }
    ;

    _proto.isWithinBounds = function isWithinBounds(bbox) {
      return this.mouseX >= bbox.left && this.mouseX <= bbox.left + bbox.width && this.mouseY >= bbox.top && this.mouseY <= bbox.top + bbox.height;
    };

    return VesuvioBar;
  }();
  VesuvioBar.defaultOptions = {
    autoHide: true,
    forceVisible: false,
    classNames: {
      contentEl: 'vlscrollbar-content',
      contentWrapper: 'vlscrollbar-content-wrapper',
      offset: 'vlscrollbar-offset',
      mask: 'vlscrollbar-mask',
      wrapper: 'vlscrollbar-wrapper',
      placeholder: 'vlscrollbar-placeholder',
      scrollbar: 'vlscrollbar-scrollbar',
      track: 'vlscrollbar-track',
      heightAutoObserverWrapperEl: 'vlscrollbar-height-auto-observer-wrapper',
      heightAutoObserverEl: 'vlscrollbar-height-auto-observer',
      visible: 'vlscrollbar-visible',
      horizontal: 'vlscrollbar-horizontal',
      vertical: 'vlscrollbar-vertical',
      hover: 'vlscrollbar-hover',
      dragging: 'vlscrollbar-dragging'
    },
    scrollbarMinSize: 25,
    scrollbarMaxSize: 0,
    timeout: 1000
  };
  VesuvioBar.instances = new WeakMap();

  if (canUseDom) {
    VesuvioBar.initHtmlApi();
  }

  return VesuvioBar;

}));