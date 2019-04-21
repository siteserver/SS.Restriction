var $api = axios.create({
  baseURL: utils.getQueryString('apiUrl') + '/' + utils.getQueryString('pluginId') + '/pages/list/',
  withCredentials: true
});

var data = {
  type: utils.getQueryString('type'),
  pageLoad: false,
  pageAlert: null,
  list: null
};

var methods = {
  apiGetList: function () {
    var $this = this;

    $api.get('', {
      params: {
        type: this.type
      }
    }).then(function (response) {
      var res = response.data;

      $this.list = res.value;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  apiDelete: function(restriction) {
    var $this = this;

    utils.loading(true);
    $api.delete('', {
      data: {
        type: this.type,
        restriction: restriction
      }
    }).then(function (response) {
      var res = response.data;

      $this.list = res.value;
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
        $this.submit();
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
  },

  btnAddClick: function() {
    utils.openLayer({
      title: '添加IP访问规则',
      url: utils.getPageUrl('ip.html') + '&type=' + this.type
    })
  },

  btnEditClick: function(restriction) {
    utils.openLayer({
      title: '添加IP访问规则',
      url: utils.getPageUrl('ip.html') + '&type=' + this.type + '&before=' + restriction
    })
  },

  btnDeleteClick: function(restriction) {
    var $this = this;
    utils.alertDelete({
      title: '删除IP访问规则',
      text: 'IP访问规则：' + restriction,
      callback: function() {
        $this.apiDelete(restriction);
      }
    })
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGetList();
  }
});
