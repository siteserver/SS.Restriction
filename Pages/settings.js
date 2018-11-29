var $url = '/settings';

var data = {
  pageLoad: false,
  pageAlert: null,
  config: null
};

var methods = {
  submit: function () {
    var $this = this;

    var payload = {
      siteId: config.siteId,
      channelId: config.channelId,
      contentId: config.contentId,
      formId: config.formId,
      type: this.pageType
    };
    if (this.pageType === 'isClosed') {
      payload.isClosed = this.formInfo.additional.isClosed;
    } else if (this.pageType === 'title') {
      payload.title = this.formInfo.title;
    } else if (this.pageType === 'description') {
      payload.description = this.formInfo.description;
    } else if (this.pageType === 'isReply') {
      payload.isReply = this.formInfo.isReply;
    } else if (this.pageType === 'isTimeout') {
      payload.isTimeout = this.formInfo.additional.isTimeout;
      payload.timeToStart = this.formInfo.additional.timeToStart;
      payload.timeToEnd = this.formInfo.additional.timeToEnd;
    } else if (this.pageType === 'isCaptcha') {
      payload.isCaptcha = this.formInfo.additional.isCaptcha;
    } else if (this.pageType === 'isAdministratorSmsNotify') {
      payload.isAdministratorSmsNotify = this.formInfo.additional.isAdministratorSmsNotify;
      payload.administratorSmsNotifyTplId = this.formInfo.additional.administratorSmsNotifyTplId;
      payload.administratorSmsNotifyKeys = this.administratorSmsNotifyKeys.join(',');
      payload.administratorSmsNotifyMobile = this.formInfo.additional.administratorSmsNotifyMobile;
    }

    utils.loading(true);
    $api.post(payload, function (err, res) {
      utils.loading(false);
      if (err) {
        $this.pageAlert = {
          type: 'danger',
          html: err.message
        };
        return;
      }

      $this.pageType = 'list';

      alert({
        toast: true,
        type: 'success',
        title: "设置保存成功",
        showConfirmButton: false,
        timer: 2000
      });
    });
  },

  btnLayerClick: function (options) {
    this.pageAlert = null;
    var url = "pages/contentAddLayer" +
      options.name +
      ".html?siteId=" +
      this.site.id +
      "&channelId=" +
      this.channel.id;

    if (options.contentId) {
      url += "&contentId=" + options.contentId
    }

    if (options.args) {
      _.forIn(options.args, function (value, key) {
        url += "&" + key + "=" + encodeURIComponent(value);
      });
    }

    utils.openLayer({
      title: options.title,
      url: url,
      full: options.full,
      width: options.width ? options.width : 700,
      height: options.height ? options.height : 500
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.pageAlert = null;

    this.$validator.validate().then(function (result) {
      if (result) {
        $this.submit();
      }
    });
  },

  getAttributeText: function (attributeName) {
    if (attributeName === 'AddDate') {
      return '添加时间';
    }

    return attributeName;
  },

  btnNavClick: function (pageName) {
    location.href = pageName + '?siteId=' + config.siteId + '&channelId=' + config.channelId + '&contentId=' + config.contentId + '&formId=' + config.formId + '&apiUrl=' + encodeURIComponent(config.apiUrl) + '&returnUrl=' + encodeURIComponent(config.returnUrl);
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    var $this = this;

    $api.get($url).then(function (response) {
        var res = response.data;
        $this.config = res.value;
      })
      .catch(function (error) {
        $this.pageAlert = utils.getPageAlert(error);
      })
      .then(function () {
        $this.pageLoad = true;
      });
  }
});