var $api = axios.create({
  baseURL: utils.getQueryString('apiUrl') + '/' + utils.getQueryString('pluginId') + '/pages/ip/',
  withCredentials: true
});

var data = {
  type: utils.getQueryString('type'),
  before: utils.getQueryString('before'),
  pageLoad: false,
  pageAlert: null,
  ipAddress: null,
  restriction: utils.getQueryString('before')
};

var methods = {
  apiGetIpAddress: function () {
    var $this = this;

    $api.get('').then(function (response) {
      var res = response.data;

      $this.ipAddress = res.value;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  apiAddRestriction: function () {
    var $this = this;

    utils.loading(true);
    $api.post('', {
      type: this.type,
      restriction: this.restriction
    }).then(function (response) {
      parent.$vue.apiGetList();
      parent.swal2({
        toast: true,
        type: 'success',
        title: "设置保存成功",
        showConfirmButton: false,
        timer: 2000
      });
      utils.closeLayer();
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  apiEditRestriction: function () {
    var $this = this;

    utils.loading(true);
    $api.put('', {
      type: this.type,
      before: this.before,
      now: this.restriction
    }).then(function (response) {
      parent.$vue.apiGetList();
      parent.swal2({
        toast: true,
        type: 'success',
        title: "设置保存成功",
        showConfirmButton: false,
        timer: 2000
      });
      utils.closeLayer();
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
        if ($this.before) {
          $this.apiEditRestriction();
        } else {
          $this.apiAddRestriction();
        }
      }
    });
  },

  btnCancelClick: function () {
    utils.closeLayer();
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
    this.apiGetIpAddress();
  }
});
