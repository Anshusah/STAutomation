function Vesuvio3D(stage_args) {
    function Stage(_a) {
        var stage = this;
        function Cursor() {
            this.grab = function () { return 'grab'; };
            this.grabbing = function () { return 'grabbing'; }
            this.pointer = function () { return 'pointer'; }
            this.default = function () { return 'default'; }
            this.active = null;
            this.old = null;
        }
        this.isValid = false;
        this.cursors = new Cursor();
        this.point3D = function (_a, _b, _c) {
            function Point3d(a, c, b) {
                this.x = (a === undefined) ? 0 : a;
                this.y = (c === undefined) ? 0 : c;
                this.z = (b === undefined) ? 0 : b;
                this.fl = 250;
                this.vpX = 0;
                this.vpY = 0;
                this.cX = 0;
                this.cY = 0;
                this.cZ = 0
                this.data = null;
            }
            Point3d.prototype = {
                setVanishingPoint: function (b, a) {
                    this.vpX = b;
                    this.vpY = a
                },
                setCenter: function (c, b, a) {
                    this.cX = c;
                    this.cY = b;
                    this.cZ = a
                },
                rotateX: function (a) {
                    var d = Math.cos(a),
                        b = Math.sin(a),
                        c = this.y * d - this.z * b,
                        e = this.z * d + this.y * b;
                    this.y = c;
                    this.z = e;
                    return this
                },
                rotateY: function (a) {
                    var d = Math.cos(a),
                        c = Math.sin(a),
                        b = this.x * d - this.z * c,
                        e = this.z * d + this.x * c;
                    this.x = b;
                    this.z = e;
                    return this
                },
                rotateZ: function (a) {
                    var d = Math.cos(a),
                        b = Math.sin(a),
                        c = this.x * d - this.y * b,
                        e = this.y * d + this.x * b;
                    this.x = c;
                    this.y = e;
                    return this
                },
                getScreenX: function () {
                    var scale = this.fl / (this.fl + this.z + this.cZ);
                    return this.vpX + (this.cX + this.x) * scale;
                },
                getScreenY: function () {
                    var scale = this.fl / (this.fl + this.z + this.cZ);
                    return this.vpY + (this.cY + this.y) * scale;
                }
            };
            return new Point3d(_a, _b, _c);
        }
        this.animate = function () {
            var Animate = function ({ easing, draw, duration, onComplete }) {
                var _self = this;
                let _start = performance.now();
                let _easing = easing;
                let _draw = draw;
                let _duration = duration;
                function animate(time) {
                    let timeFraction = (time - _start) / _duration;
                    if (timeFraction > 1) timeFraction = 1;
                    let progress = easing(timeFraction);
                    if (timeFraction < 1) {
                        setTimeout(function () {
                            _draw.call(_self, progress);
                            requestAnimationFrame(animate);
                        }.bind(this), 1000 / stage.fps);
                    } else if (typeof onComplete === "function" && timeFraction >= 1) {
                        onComplete.call(_self);
                    }
                }
                _draw.call(_self, false);
                requestAnimationFrame(animate);
                return _self;
            }
            return Animate;
        }

        function Utils(stage) {
            var self = stage;
            this.map = function (a, b, c, d, e) {
                return (a - b) / (c - b) * (e - d) + d;
            };
            this.compareObjects = function (x,y) {
               
                if (x === y) return true;

                if (!(x instanceof Object) || !(y instanceof Object)) return false;

                if (x.constructor !== y.constructor) return false;

                for (var p in x) {
                    if (!x.hasOwnProperty(p)) continue;

                    if (!y.hasOwnProperty(p)) return false;
                    
                    if (x[p] === y[p]) continue;
                    
                    if (typeof (x[p]) !== "object") return false;
                    
                    if (!Object.equals(x[p], y[p])) return false;
                    
                }

                for (p in y) {
                    if (y.hasOwnProperty(p) && !x.hasOwnProperty(p)) return false;
                }
                return true;
                
            };
            this.intersect = function (line1StartX, line1StartY, line1EndX, line1EndY, line2StartX, line2StartY, line2EndX, line2EndY) {
                var denominator, a, b, numerator1, numerator2, result = {
                    x: null,
                    y: null,
                    onLine1: false,
                    onLine2: false
                };
                denominator = ((line2EndY - line2StartY) * (line1EndX - line1StartX)) - ((line2EndX - line2StartX) * (line1EndY - line1StartY));
                if (denominator == 0) {
                    return result;
                }
                a = line1StartY - line2StartY;
                b = line1StartX - line2StartX;
                numerator1 = ((line2EndX - line2StartX) * a) - ((line2EndY - line2StartY) * b);
                numerator2 = ((line1EndX - line1StartX) * a) - ((line1EndY - line1StartY) * b);
                a = numerator1 / denominator;
                b = numerator2 / denominator;

                result.x = line1StartX + (a * (line1EndX - line1StartX));
                result.y = line1StartY + (a * (line1EndY - line1StartY));
                if (a > 0 && a < 1) {
                    result.onLine1 = true;
                }
                if (b > 0 && b < 1) {
                    result.onLine2 = true;
                }
                return result;
            };
            this.selector = function (args) {
                var Stage = this;
                function Selector(selector_args) {
                    var Selector = this;
                    this.margin = 10;
                    this.selectionOffset = selector_args.selectionOffset || 0;
                    this.show = selector_args.show || false;
                    this.element = selector_args.element || null;
                    this.width = selector_args.width ? (selector_args.width + this.margin + this.element.lineWidth) : this.element.width + this.margin;
                    this.height = selector_args.height ? (selector_args.height + this.margin + this.element.lineWidth) : this.element.height + this.margin;
                    this.x = selector_args.x - this.width / 2 || this.element.lineWidth - this.width / 2;
                    this.y = selector_args.y - this.height / 2 || this.element.lineWidth - this.height / 2;
                    this.points = selector_args.points || null;
                    this.pointObject = [];
                    this.oldCursor = null;
                    function Position(selector) {
                        var self = selector
                        var selectorLineWidth = 1;
                        this.constantX = selectorLineWidth;
                        this.constantY = selectorLineWidth
                        this.x = selector.x;
                        this.y = selector.y;
                        this.width = selector.width;
                        this.height = selector.height;
                        this.oldCursor = null;
                        this.activePoint = null;
                        this.LT = {
                            x: function () {
                                return this.x - this.constantX;
                            }.bind(this),
                            y: function () {
                                return this.y - this.constantY;
                            }.bind(this),
                            updatePoints: function (point, selector) {
                                this.activePoint = point;
                                var posX = 0, posY = 0, width = 0, height = 0, prevX = 0, prevY = 0;
                                posX = Stage.mouse.move.stageX;
                                posY = Stage.mouse.move.stageY;
                                prevX = (self.x + self.width);
                                prevY = (self.y + self.height);
                                width = prevX - posX;
                                height = prevY - posY;
                                width = (width < self.margin) ? self.margin : width;
                                height = (height < self.margin) ? self.margin : height;
                                if (!self.change.is) {
                                    if (width < height) {
                                        width = height;
                                    } else {
                                        height = width;
                                    }
                                    self.change.width = width;
                                    self.change.height = height;
                                    self.change.x = posX;
                                    self.change.y = posY;
                                    self.change.is = true;
                                }

                            }.bind(this),
                            cursor: function () {
                                return "nwse-resize";
                            }
                        };
                        this.TC = {
                            x: function () {
                                return this.x + this.width / 2 - this.constantY;
                            }.bind(this),
                            y: function () {
                                return this.y - this.constantY;
                            }.bind(this),
                            updatePoints: function (point) {
                                this.activePoint = point;
                                var posY = 0, height = 0, prevX = 0, prevY = 0;
                                posY = Stage.mouse.move.stageY;
                                prevY = (self.y + self.height);
                                height = prevY - posY;
                                height = (height < self.margin) ? self.margin : height;
                                if (!self.change.is) {
                                    self.change.width = self.width;
                                    self.change.height = height;
                                    self.change.x = self.x;
                                    self.change.y = posY;
                                    self.change.is = true;
                                }
                            }.bind(this),
                            cursor: function () {
                                return "ns-resize";
                            }
                        };
                        this.TR = {
                            x: function () {
                                var cord = this.x + this.width;
                                return cord;
                            }.bind(this),
                            y: function () {
                                var cord = this.y - this.constantY;
                                return cord;
                            }.bind(this),
                            updatePoints: function (point, selector) {

                                this.activePoint = point;
                                var posX = 0, posY = 0, width = 0, height = 0, prevX = 0, prevY = 0;
                                posX = Stage.mouse.move.stageX;
                                posY = Stage.mouse.move.stageY;
                                prevX = (self.x + self.width);
                                prevY = (self.y + self.height);
                                width = prevX - posX;
                                height = prevY - posY;
                                width = (width < self.margin) ? self.margin : width;
                                height = (height < self.margin) ? self.margin : height;
                                if (!self.change.is) {
                                    if (width < height) {
                                        width = height;
                                    } else {
                                        height = width;
                                    }
                                    self.change.width = width;
                                    self.change.height = height;
                                    self.change.x = self.x;
                                    self.change.y = posY;
                                    self.change.is = true;
                                }

                            }.bind(this),
                            cursor: function () {
                                return "nesw-resize";
                            }
                        };
                        this.RM = {
                            x: function () {
                                return this.x + this.width;
                            }.bind(this),
                            y: function () {
                                return this.y + this.height / 2 - this.constantY;
                            }.bind(this),
                            updatePoints: function (point, selector) {
                                this.activePoint = point;
                                var posX = 0, width = 0, prevX = 0;
                                posX = Stage.mouse.move.stageX;
                                prevX = self.x;
                                width = posX - prevX;
                                width = (width < self.margin) ? self.margin : width;
                                if (!self.change.is) {
                                    self.change.width = width;
                                    self.change.height = self.height;
                                    self.change.x = self.x;
                                    self.change.y = self.y;
                                    self.change.is = true;
                                }
                            }.bind(this),
                            cursor: function () {
                                return "col-resize";
                            }
                        }
                        this.RB = {
                            x: function () {
                                return this.x + this.width;
                            }.bind(this),
                            y: function () {
                                return this.y + this.height;
                            }.bind(this),
                            updatePoints: function (point, selector) {
                                if (!self.change.is) {
                                    this.activePoint = point;
                                    var width = (Stage.mouse.move.stageX) - (self.x);
                                    var height = (Stage.mouse.move.stageY) - (self.y);
                                    if (width <= self.margin) {
                                        width = self.margin;
                                    }
                                    if (height <= self.margin) {
                                        height = self.margin;
                                    }
                                    if (width > height) {
                                        height = width;
                                    } else {
                                        width = height;
                                    }
                                    self.change.width = width;
                                    self.change.height = height;
                                    self.change.x = self.x;
                                    self.change.y = self.y;
                                    self.pointPosition.x = self.change.x;
                                    self.pointPosition.y = self.change.y;
                                    self.pointPosition.width = self.change.width;
                                    self.pointPosition.height = self.change.width;

                                    self.change.is = true;

                                }
                            }.bind(this),
                            cursor: function () {
                                return "nwse-resize";
                            }
                        },
                            this.BC = {
                                x: function () {
                                    return this.x + this.width / 2 - this.constantX;
                                }.bind(this),
                                y: function () {
                                    return this.y + this.height;
                                }.bind(this),
                                updatePoints: function (point) {
                                    this.activePoint = point;
                                    var posY = 0, height = 0, prevY = 0;
                                    posY = Stage.mouse.move.stageY;
                                    prevY = self.y;
                                    height = posY - prevY;
                                    height = (height < self.margin) ? self.margin : height;
                                    if (!self.change.is) {
                                        self.change.width = self.width;
                                        self.change.height = height;
                                        self.change.x = self.x;
                                        self.change.y = self.y;
                                        self.change.is = true;
                                    }
                                }.bind(this),
                                cursor: function () {
                                    return "ns-resize";
                                }
                            },
                            this.BL = {
                                x: function () {
                                    return this.x - this.constantX;
                                }.bind(this),
                                y: function () {
                                    return this.y + this.height;
                                }.bind(this),
                                updatePoints: function (point) {
                                    if (!self.change.is) {
                                        this.activePoint = point;
                                        var posX = 0, posY = 0, width = 0, height = 0, prevX = 0, prevY = 0;
                                        posX = Stage.mouse.move.stageX;
                                        posY = Stage.mouse.move.stageY;
                                        prevX = (self.x + self.width);
                                        prevY = (self.y + self.height);
                                        width = prevX - posX;
                                        height = prevY - posY;
                                        width = (width < self.margin) ? self.margin : width;
                                        height = (height < self.margin) ? self.margin : height;
                                        if (!self.change.is) {
                                            if (width < height) {
                                                width = height;
                                            } else {
                                                height = width;
                                            }
                                            self.change.width = width;
                                            self.change.height = height;
                                            self.change.x = posX;
                                            self.change.y = this.y;
                                            self.change.is = true;
                                        }
                                        self.change.is = true;
                                    }
                                }.bind(this),
                                cursor: function () {
                                    return "nesw-resize";
                                }
                            },
                            this.LC = {
                                x: function () {
                                    return this.x - this.constantX;
                                }.bind(this),
                                y: function () {
                                    return this.y + this.height / 2 - this.constantY;
                                }.bind(this),
                                updatePoints: function (point) {
                                    this.activePoint = point;
                                    var posX = 0, width = 0, prevX = 0;
                                    posX = Stage.mouse.move.stageX;
                                    prevX = self.x + self.width;
                                    width = prevX - posX;
                                    width = (width < self.margin) ? self.margin : width;
                                    if (!self.change.is) {
                                        self.change.width = width;
                                        self.change.height = self.height;
                                        self.change.x = posX;
                                        self.change.y = self.y;
                                        self.change.is = true;
                                    }
                                }.bind(this),
                                cursor: function () {
                                    return "col-resize";

                                },
                            };
                    }
                    this.pointPosition = new Position(this);
                    this.getPoint = function (_a) {
                        function Point(_ars) {
                            this.show = _ars.show;
                            this.selector = _ars.selector || null
                            this.x = _ars.x || 0;
                            this.y = _ars.y || 0;
                            this.isLocked = true;
                            this.fill = _ars.fill || "#3674c5";
                            this.id = _ars.id || Stage.generateId();
                            this.type = _ars.type || "arc";
                            this.radius = _ars.radius || 3.5;
                            this.width = _ars.width || 4;
                            this.height = _ars.height || 4;
                            this.inPath = false;
                            this.countDown = null;
                            this.render = function () {
                                var self = this;
                                if (this.countDown != null && this.countDown > 0) {
                                    this.countDown--;
                                }
                                if (this.selector.defaultPoints[self.id] && this.selector.show) {
                                    Stage.ctx2d.beginPath();
                                    Stage.ctx2d.lineWidth = 0;
                                    Stage.ctx2d.fillStyle = this.fill;
                                    Stage.ctx2d.globalAlpha = 0.8;
                                    if (this.type == "arc") {
                                        Stage.ctx2d.arc(
                                            self.selector.pointPosition[self.id].x(self, self.selector),
                                            self.selector.pointPosition[self.id].y(self, self.selector),
                                            self.radius,
                                            0,
                                            Math.PI * 2
                                        );
                                    }
                                    Stage.ctx2d.fill();
                                    Stage.ctx2d.globalAlpha = 1;
                                    this.inPath = Stage.ctx2d.isPointInPath(Stage.mouse.move.mouseX, Stage.mouse.move.mouseY);
                                    if (this.inPath) {
                                        this.countDown = 10;
                                        if (self.selector.pointPosition.oldCursor == null) {
                                            var c = Stage.canvas.style.cursor != "" ? Stage.canvas.style.cursor : "default";
                                            self.selector.pointPosition.oldCursor = c;
                                            Stage.canvas.style.cursor = self.selector.pointPosition[self.id].cursor();
                                            this.isLocked = false;
                                        }
                                    } else {
                                        if (!Stage.mouse.isDown && this.countDown == 0 && self.selector.pointPosition.oldCursor != null) {
                                            Stage.canvas.style.cursor = self.selector.pointPosition.oldCursor;
                                            this.countDown = null;
                                            self.selector.pointPosition.oldCursor = null
                                            this.isLocked = true;
                                        }
                                    }
                                    if (Stage.mouse.isDown && !this.isLocked) {
                                        self.selector.pointPosition[self.id].updatePoints(self, self.selector);

                                    }
                                    Stage.ctx2d.closePath();
                                }
                                return this;
                            }

                        }
                        return new Point(_a).render();
                    };
                    this.defaultPoints = {
                        LT: true,// Left Top
                        TC: true,// Top Center
                        TR: true,// Top Right
                        RM: true,// Top Mid
                        RB: true,// Right Bottom
                        BC: true,// Bottom Center
                        BL: true,// Bottom Left
                        LC: true // Left Center
                    };
                    this.isLocked = true;
                    this.increaseSelectionOffset = true;
                    this.change = {
                        is: false,
                        width: 0,
                        height: 0,
                        x: 0,
                        y: 0
                    }
                    this.inPath = false;
                    this.isEntered = false;
                    Stage.mouse.onMove(function (mouse, stage) {
                        //this.increaseSelectionOffset=false;
                        //this.render(Stage,Stage.ctx2d);	
                        //this.increaseSelectionOffset=true;
                        if (this.inPath && this.oldCursor == null && !Stage.mouse.isDown) {
                            this.oldCursor = Stage.canvas.style.cursor;
                        }

                        if (!this.inPath && this.oldCursor != null) {
                            Stage.canvas.style.cursor = Stage.cursors.default();
                            this.oldCursor = null;
                        }
                    }.bind(this));
                    Stage.mouse.onDown(function (e) {
                        this.increaseSelectionOffset = false;
                        this.render(Stage, Stage.ctx2d);
                        this.increaseSelectionOffset = true;
                        if (this.inPath && Stage.mouse.isDown) {
                            this.isLocked = false
                        }
                        if (this.inPath) {
                            stage.mouse.down.dragXOff = this.x - e.stageX;
                            stage.mouse.down.dragYOff = this.y - e.stageY;
                        }

                    }.bind(this));
                    Stage.mouse.onUp(function (e) {
                        if (!this.isLocked) {
                            this.isLocked = true;
                        }
                        this.increaseSelectionOffset = false;
                        this.render(Stage, Stage.ctx2d);
                        this.increaseSelectionOffset = true;
                        if (!this.inPath && this.oldCursor != null) {
                            if (Stage.mouse.isDown) {
                                Stage.canvas.style.cursor = Stage.cursors.grabbing();
                            } else {
                                Stage.canvas.style.cursor = Stage.cursors.grab();
                            }
                            this.oldCursor = null;
                        }
                    }.bind(this));
                    Stage.mouse.onClick(function (e) {
                        if (this.inPath) {
                            this.show = this.show ? false : true;
                           // console.log(this.element);
                            if (typeof (this.element.onClick) == 'function') {
                                this.element.onClick.call(this.element, e)
                            }
                        } else {

                            this.show = false;
                        }
                    }.bind(this));
                    this.render = function (stage, ctx) {

                        if (this.change.is && this.isLocked) {
                            this.width = this.change.width;
                            this.height = this.change.height;
                            this.x = this.change.x;
                            this.y = this.change.y;
                            this.pointPosition.x = this.change.x;
                            this.pointPosition.y = this.change.y;
                            this.pointPosition.width = this.change.width;
                            this.pointPosition.height = this.change.height;
                            this.element.update(this.change, this);
                            this.change.is = false;
                        }
                        if (!this.isLocked) {
                            this.change.x = (stage.mouse.move.stageX) + (stage.mouse.down.dragXOff);
                            this.change.y = (stage.mouse.move.stageY) + (stage.mouse.down.dragYOff);
                            this.change.width = this.width;
                            this.change.height = this.height;
                            this.x = this.change.x;
                            this.y = this.change.y;

                            this.pointPosition.x = this.change.x;
                            this.pointPosition.y = this.change.y;
                            this.pointPosition.width = this.change.width;
                            this.pointPosition.height = this.change.height;
                            this.element.update(this.change, this);


                        }


                        var lineWidth = this.element.lineWidth || 0,
                            borderX = this.x - lineWidth,
                            borderY = this.y - lineWidth,
                            borderWidth = this.width + lineWidth,
                            borderHeight = this.height + lineWidth;
                        if (
                            (stage.mouse.move.stageX - this.margin + 1) >= (this.x) &&
                            (stage.mouse.move.stageX - this.margin + 1) <= ((this.x + this.width) - this.margin * 2) &&
                            (stage.mouse.move.stageY - this.margin + 1) >= (this.y) &&
                            (stage.mouse.move.stageY - this.margin + 1) <= ((this.y + this.height) - this.margin * 2)
                        ) {
                            this.inPath = true;
                            if (!this.isEntered) {
                                this.isEntered = true;
                                if (typeof (this.element.onMouseEnter) == 'function') {
                                    this.element.onMouseEnter.call(this.element, stage.mouse.move)
                                }
                            }

                        } else {
                            this.inPath = false;
                            if (this.isEntered) {
                                this.isEntered = false;
                                if (typeof (this.element.onMouseLeave) == 'function') {
                                    this.element.onMouseLeave.call(this.element, stage.mouse.move)
                                }
                            }
                        }
                        if (this.inPath && (this.element.draggable || this.element.resizable) && this.oldCursor != null) {
                            if (Stage.mouse.isDown) {
                                Stage.canvas.style.cursor = Stage.cursors.grabbing();
                            } else {
                                Stage.canvas.style.cursor = Stage.cursors.grab();
                            }
                        }
                        if (this.increaseSelectionOffset) {
                            this.selectionOffset++;
                            if (parseInt(this.selectionOffset) > 16) {
                                this.selectionOffset = -6;
                            }
                            stage.ctx2d.beginPath();
                            stage.ctx2d.strokeStyle = "#5c5d5c";
                            stage.ctx2d.lineWidth = 1;
                            stage.ctx2d.setLineDash([6, 6]);
                            if (this.show) {
                                stage.ctx2d.lineDashOffset = -parseInt(this.selectionOffset);
                                stage.ctx2d.strokeRect(borderX, borderY, borderWidth, borderHeight);
                            }


                            stage.ctx2d.setLineDash([]);
                            stage.ctx2d.closePath();

                            this.pointObject.forEach(function (item) {
                                item.render();
                            }.bind(this));

                        }

                    }
                    this.init = function () {
                        var Self = this;
                        if (this.points) {
                            Object.keys(this.points).forEach(function (e) {
                                Self.defaultPoints[e] = Self.points[e];
                            });
                        }
                        if (Self.defaultPoints) {
                            Object.keys(Self.defaultPoints).forEach(function (e) {
                                Self.pointObject.push(Self.getPoint(
                                    {
                                        selector: Selector,
                                        id: e,
                                        show: Self.defaultPoints[e],
                                        x: Selector.x,
                                        y: Selector.y,
                                        width: Selector.width,
                                        height: Selector.width,
                                    }
                                ));
                            });
                        }
                        return this;
                    }
                }
                return new Selector(args).init();
            }.bind(self),
            this.getDpi = function () {
                var devicePixelRatio = window.devicePixelRatio || 1, backingStoreRatio = this.canvas.webkitBackingStorePixelRatio ||
                    this.canvas.mozBackingStorePixelRatio ||
                    this.canvas.msBackingStorePixelRatio ||
                    this.canvas.oBackingStorePixelRatio ||
                    this.canvas.backingStorePixelRatio || 1;
                return dpi = devicePixelRatio / backingStoreRatio;
            }.bind(self),
            this.generateId = function () {
                return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                    return v.toString(16);
                });
            }.bind(self),
            this.toDegree = function (radians) {

                return radians * (180 / Math.PI);
            },
            this.toRadian = function (degree) {

                return degree * (Math.PI / 180);

            };
            return this;
        };
        this.getMouse = function (e) {

            var element = this.canvas, offsetX = 0, offsetY = 0, mx, my;
            var self = this;
            if (!element) {
                return { mouseX: 0, mouseY: 0, pageX: 0, pageY: 0, stageX: 0, stageY: 0 };
            }
            var rect = this.canvas.getBoundingClientRect();
            this.offsetX = rect.left;
            this.offsetY = rect.top;
            var ob = { mouseX: 0, mouseY: 0, pageX: 0, pageY: 0, stageX: 0, stageY: 0 };
            ob.stageX = e.pageX - this.offsetX;
            ob.stageY = e.pageY - this.offsetY;
            //====-------------------------
            ob.mouseX = e.pageX - this.offsetX;
            ob.mouseY = e.pageY - this.offsetY;
            //====-------------------------
            ob.pageX = e.pageX;//-this.offsetX;
            ob.pageY = e.pageY;//-this.offsetY;	
            return ob;
        }
        function Mouse(stage) {
            var m = stage.getMouse();
            this.clickList = [];
            this.downList = [];
            this.upList = [];
            this.moveList = [];
            this.click = m;
            this.down = m;
            this.dblclick = m;
            this.dblclickList = [];
            this.up = m;
            this.move = m;
            this.isDown = false;
            this.lastX = 0;
            this.lastY = 0;
            this.time = 0;
            this.onClick = function (e) {
                this.clickList.push(e);
            };
            this.onDown = function (e) {
                this.downList.push(e);
            };
            this.onUp = function (e) {
                this.upList.push(e);
            };

            this.onDblclick = function (e) {
                this.dblclickList.push(e);
            };
            
        }
        this.utils = new Utils(stage);
        this.mouse = new Mouse(stage);
        this.id = _a.id || Math.random().toString(36).substr(2, 5);
        this.width = _a.width || 400;
        this.height = _a.height || 400;
        this.elements = [];
        var extend = function (options) {
			/*var extended = {} , prop;
			for (prop in defaults) {
			  if (Object.prototype.hasOwnProperty.call(defaults, prop))
				extended[prop] = defaults[prop];
			}
			for (prop in options) {
			  if (Object.prototype.hasOwnProperty.call(options, prop))
				extended[prop] = options[prop];
			}
			*/
            options['stage'] = stage;
            options['ctx'] = stage.ctx2d;
            return options;
        };
        this.addElement = function (obj) {
            console.log(obj)
            if (!obj.id) {
                obj.id = this.utils.generateId();
            }
            console.log("------------------");
            console.log(obj)
            obj.show = obj.show || true;
            layer = extend(obj);

            if (this.getElement(layer.id) !== false) {
                console.log('Layer already exists');
                console.log(layer);
                return false;
            }

            this.elements.push(layer);
            return this;
        };
        this.getElement = function (id) {
            if (id == null) return this.elements;
            var length = this.elements.length;
            for (var i = 0; i < length; i++) {
                if (this.elements[i].id == id)
                    return this.elements[i];
            }
            return false;
        };
        this.removeElement = function (id) {
            var length = this.elements.length;
            for (var i = 0; i < length; i++) {
                if (this.elements[i].id === id) {
                    removed = this.elements[i];
                    this.elements.splice(i, 1);
                    return removed;
                }
            }
            return false;
        };
        this.isInit = false;
        this.render = function () {

            var canvas = this.canvas;
            var ctx = this.ctx2d;
            var stage = this;
            this.clear();
            if (!this.isInit) {
                this.isInit = true;
                this.elements.forEach(function (item, index, array) {
                    try {
                        item.init(stage, canvas, ctx);
                    } catch (e) {
                        console.warn(e);
                    }


                })
                return this;
            }
            this.elements.forEach(function (item, index, array) {
                if (item.show) {
                    if (item.opacity != null) {
                        ctx.globalAlpha = item.opacity;
                    }

                    item.render(stage, canvas, ctx);
                    ctx.globalAlpha = 1;
                    if (item.selector != null) {
                        item.selector.render(stage, canvas, ctx);
                    }
                }
            });
            return this;
        };
        this.clear = function () {
            this.ctx2d.clearRect(0, 0, this.canvas.width*2, this.canvas.height*2);
        };
        this.fps = 60;
        this.keyFrame = function () {
            var self = this;
            setTimeout(function () {
                self.render();
               requestAnimationFrame(self.keyFrame);
            }.bind(this), 1000 / self.fps);
            return this;

        }.bind(this);
        this.canvas = document.getElementById(this.id);
        if (!this.canvas) {
            this.canvas = document.createElement("canvas");
            this.canvas.id = this.id;
            document.body.appendChild(this.canvas);
        }
        this.ctx2d = this.canvas.getContext('2d');
        this.canvas.style.width = this.width + "px";
        this.canvas.style.height = this.height + "px";
        if (this.utils.getDpi() > 0) {
            this.canvas.width = this.width * this.utils.getDpi();
            this.canvas.height = this.height * this.utils.getDpi();
            this.ctx2d.scale(this.utils.getDpi(), this.utils.getDpi());
        }
        this.ctx2d.fillStyle = this.fill;
        this.ctx2d.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.keyFrame();
        this.canvas.onmousedown = function (e) {
            if (!this.isValid) return false;
            this.downTime = +new Date();
            var m = this.getMouse(e);
            this.mouse.down = m
            this.mouse.isDown = true;
            setTimeout(function () {
                if (!this.mouse.isDown) {
                    if (this.mouse.clickList.length) {
                        this.mouse.clickList.forEach(function (f) {
                            f.call(this, m);
                        })
                    }
                } else {
                    if (this.mouse.downList.length) {
                        this.mouse.downList.forEach(function (f) {
                            f.call(this, m);
                        })
                    }
                }
            }.bind(this), 150);

        }.bind(this);
        this.canvas.onmouseup = function (e) {
            if (!this.isValid) return false;
            this.upTime = +new Date();
            this.mouse.time = parseFloat(this.upTime - this.downTime);
            var m = this.getMouse(e);
            this.mouse.up = m;
            this.mouse.isDown = false;
            if (this.mouse.upList.length) {
                this.mouse.upList.forEach(function (f) {
                    f.call(this, m);
                }.bind(this))
            }

        }.bind(this);
        this.canvas.onmousemove = function (e) {
            if (!this.isValid) return false;
            var m = this.getMouse(e);

            this.mouse.move = m;
            if (this.mouse.moveList.length) {
                this.mouse.moveList.forEach(function (f) {
                    f.call(this, m);
                })
            }
        }.bind(this);
        this.canvas.ondblclick = function (e) {
            if (!this.isValid) return false;
            
            var m = this.getMouse(e);
            this.mouse.dblclick = m
            if (this.mouse.dblclickList.length) {
                this.mouse.dblclickList.forEach(function (f) {
                    f.call(this, m);
                })
            }
        }.bind(this);
        this.getMouse = function (e) {

            var element = this.canvas, offsetX = 0, offsetY = 0, mx, my;
            var self = stage;
            var ob = { stageX: 0, stageY: 0, mouseX: 0, mouseY: 0, pageX: 0, pageY: 0, dpi: self.utils.getDpi(), dragXOff: 0, dragYOff: 0 };

            if (e == undefined) {
                ob.dpi = self.utils.getDpi();
                return ob;
            }

            var rect = this.canvas.getBoundingClientRect();
            this.offsetX = rect.left;
            this.offsetY = rect.top;


            ob.mouseX = (e.pageX - this.offsetX) * self.utils.getDpi();
            ob.mouseY = (e.pageY - this.offsetY) * self.utils.getDpi();
            ob.stageX = e.pageX - this.offsetX;
            ob.stageY = e.pageY - this.offsetY;
            ob.pageX = e.pageX;//-rect.left;
            ob.pageY = e.pageY;//-rect.top;	
            return ob;
        }
        return this;
    }
    return new Stage(stage_args);
}
var ease = {
    linear: function (t) {
        return t
    },
    easeInQuad: function (t) {
        return t * t
    },
    easeOutQuad: function (t) {
        return t * (2 - t)
    },
    easeInOutQuad: function (t) {
        return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t
    },
    easeInCubic: function (t) {
        return t * t * t
    },
    easeOutCubic: function (t) {
        return (--t) * t * t + 1
    },
    easeInOutCubic: function (t) {
        return t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1
    },
    easeInQuart: function (t) {
        return t * t * t * t
    },
    easeOutQuart: function (t) {
        return 1 - (--t) * t * t * t
    },
    easeInOutQuart: function (t) {
        return t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t
    },
    easeInQuint: function (t) {
        return t * t * t * t * t
    },
    easeOutQuint: function (t) {
        return 1 + (--t) * t * t * t * t
    },
    easeInOutQuint: function (t) {
        return t < .5 ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t
    }
};