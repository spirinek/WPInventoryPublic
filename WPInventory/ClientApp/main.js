import Vue from 'vue';
import router from './router/index';
import ElementUI from 'element-ui';
import moment from 'vue-moment';
import 'material-icons';
import 'element-ui/lib/theme-chalk/index.css';
import 'material-design-icons/iconfont/material-icons.css';
import locale from 'element-ui/lib/locale/lang/ru-RU';
import App from './App.vue';

Vue.use(ElementUI, {locale})
Vue.use(moment)
new Vue({
    el:'#app-root',
    router,
    render: h=>h(App)
})