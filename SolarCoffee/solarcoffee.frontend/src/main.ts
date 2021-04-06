import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import moment from "moment";

Vue.config.productionTip = false;

Vue.filter('price', function(number:number){
  if (isNaN(number)) {
    return '-';
  }
  return 'N ' + number.toFixed(2);
});

Vue.filter('humanizeDate', function(date: Date){
  return moment(date).format("MMM Do YYYY");
});


new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
