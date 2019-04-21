var $api = axios.create({
  baseURL: utils.getQueryString('apiUrl') + '/' + utils.getQueryString('pluginId') + '/pages/settings/',
  withCredentials: true
});

var data = {
  pageLoad: false,
  pageAlert: null,
  configInfo: null,
  host: null
};

var methods = {
  apiGetConfig: function () {
    var $this = this;

    $api.get('').then(function (response) {
      var res = response.data;

      $this.configInfo = res.value;
      if (!$this.configInfo.isHostRestriction && !$this.configInfo.host) {
        $this.configInfo.host = res.host;
      }
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  apiSetConfig: function () {
    var $this = this;

    utils.loading(true);
    $api.post('', $this.configInfo).then(function (response) {
      var res = response.data;

      swal2({
        toast: true,
        type: 'success',
        title: "设置保存成功",
        showConfirmButton: false,
        timer: 2000
      });
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.pageAlert = null;

    this.$validator.validate().then(function (result) {
      if (result) {
        $this.apiSetConfig();
      }
    });
  },

  btnNavClick: function (pageName, name, value) {
    utils.loading(true);
    var url = utils.getPageUrl(pageName);
    if (name && value) {
      url += '&' + name + '=' + encodeURIComponent(value);
    }
    location.href = url;
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGetConfig();
  }
});
