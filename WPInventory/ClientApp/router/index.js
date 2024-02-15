import Vue from 'vue';
import Router from 'vue-router';
import Home from "../Views/Home.vue"
import Settings from "../Views/Settings.vue"
import ComputerCard from "../Views/ComputerCard.vue"
import page404 from "../Views/404.vue"
Vue.use(Router);
export default new Router({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: 'Home',
            component: Home,
            meta:{title: 'Компьютеры'}
        },
        {
            path: '/settings',
            name: 'Settings',
            component: Settings,
            meta:{title: 'Настройки'}
        },
        {
            path: '/computers/:guid',
            name: 'ComputerCard',
            component: ComputerCard,
            meta:{title: 'Подробности'}
        },
        {
            path: '/404',
            name: '404',
            hidden: true,
            component: page404,
            meta:{title:'404'}
        },
        {
            hidden: true,
            path: '*',
            redirect: '/404'
        }
    ]
});