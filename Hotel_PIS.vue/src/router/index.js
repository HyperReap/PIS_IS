import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Rooms from '../views/customer/rooms.vue'
import ReservationDetails from '../views/customer/reservation-details.vue'
import MyReservations from '../views/myReservations.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    showInMenu: true
  },
  {
    path: '/rezervace',
    name: 'Nová rezervace',
    component: Rooms,
    showInMenu: true
  },
  {
    path: '/dataily-rezervace',
    name: 'Detaily rezervace',
    component: ReservationDetails,
    showInMenu: false
  },
  {
    path: '/moje-rezervace',
    name: 'Moje rezervace',
    component: MyReservations,
    showInMenu: true
  },
  {
    path: "/:pathMatch(.*)*",
    name: 'Stránka nenalezena',
    component: () => import('../views/notFound.vue'),
    showInMenu: false
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
    document.title = to.name;
    next();
});

export default router
