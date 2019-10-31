
(function ($) {
    $.fn.extend({
        test: function () {
            alert($(this).attr('id'));
        }
    })
    $.fn.extend({
        alertWhileClick: function () {
            alert($(this).val());
        }
    });
    $.fn.getLbj = function (option, agrs) {
        value = $.fn.getLbj.methods[option](this, agrs);
        return typeof value === 'undefined' || value == null ? this : value;
    }
    $.fn.getLbj.methods = {
        getValue: function (target, options) {
            alert("error");
            return true;
        },
    }
    //符合条件时增加方法
    Function.prototype.method = function (name, func) {
        if (!this.prototype[name]) {
            this.prototype[name] = func;
            return this;
        }
    }
    String.method('trim2', function () {
        return '每一次难过的时候';
        //return this.replace('/^\s+|\s+$\g', '');
    })
})(jQuery)