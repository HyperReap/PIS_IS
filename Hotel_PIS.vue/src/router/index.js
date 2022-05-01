import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Rooms from '../views/customer/rooms.vue'
import ReservationDetails from '../views/customer/reservation-details.vue'
import MyReservations from '../views/customer/my-reservations.vue'
import Failure from "@/views/failures";
import ReservationConfirmation from '../views/customer/reservation-confirmation.vue'
import Reservations from '../views/receptionist/reservations.vue'
import Login from '../views/admin/login.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
        showInMenu: true
    }
  },
  {
    path: '/rezervace',
    name: 'Nová rezervace',
    component: Rooms,
    meta: {
        showInMenu: true
    }
  },
  {
    path: '/sprava-rezervaci',
    name: 'Správa rezervací',
    component: Reservations,
    meta: {
        showInMenu: true,
    }
  },
  {
    path: '/dataily-rezervace',
    name: 'Detaily rezervace',
    component: ReservationDetails,
    meta: {
        showInMenu: false,
        parentHighlight: '/rezervace'
    }
  },
  {
    path: '/potvrzeni-rezervace',
    name: 'Potvrzení rezervace',
    component: ReservationConfirmation,
    meta: {
        showInMenu: false,
        parentHighlight: '/rezervace'
    }
  },
  {
    path: '/moje-rezervace',
    name: 'Moje rezervace',
    component: MyReservations,
    meta: {
        showInMenu: true
    }
  },
  {
    path: '/zavady',
    name: 'Závady',
    component: Failure,
    meta: {
      showInMenu: true
    }
  },
  {
    path: '/login',
    name: 'Přihlášení',
    component: Login,
    meta: {
        showInMenu: false,
        hideMenu: true,
    }
  },
  {
    path: "/:pathMatch(.*)*",
    name: 'Stránka nenalezena',
    component: () => import('../views/notFound.vue'),
    meta: {
        showInMenu: false
    }
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
