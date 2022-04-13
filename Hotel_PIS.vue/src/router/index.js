import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import MyReservations from '../views/myReservations.vue'
import Rooms from '../views/rooms.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/pokoje',
    name: 'Pokoje',
      component: Rooms
  },
  {
    path: '/moje-rezervace',
    name: 'Moje rezervace',
    component: MyReservations
  },
  {
    path: "/:pathMatch(.*)*",
    name: 'Stránka nenalezena',
    component: () => import('../views/notFound.vue')
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
