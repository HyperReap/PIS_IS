import { createRouter, createWebHistory } from 'vue-router'
import store from "../store";
import Home from '../views/Home.vue'
import Rooms from '../views/customer/rooms.vue'
import ReservationDetails from '../views/customer/reservation-details.vue'
import MyReservations from '../views/customer/my-reservations.vue'
import ReservationConfirmation from '../views/customer/reservation-confirmation.vue'
import Failure from "@/views/admin/technician/failures.vue";
import UncleanedRooms from "@/components/admin/cleaningLady/uncleanedRooms";

import StatsManagement from "@/views/admin/manager/statsManagement.vue";
import EmployeesManagement from "@/views/admin/manager/employeesManagement.vue";
import RoomsManagement from "@/views/admin/manager/roomManagement.vue";

import Login from '../views/admin/login.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
        showInMenu: true,
        acceptedUserRoles: [0, 1, 2, 3, 4]
    }
  },
  {
    path: '/sprava-pokoju',
    name: 'Správa pokojů',
    component: RoomsManagement,
    meta: {
        showInMenu: true,
        acceptedUserRoles: [0, 1]
    }
  },
  {
    path: '/sprava-zamestnancu',
    name: 'Správa zaměstnanců',
    component: EmployeesManagement,
    meta: {
        showInMenu: true,
        acceptedUserRoles: [1]
    }
  },
  {
    path: '/statistiky',
    name: 'Statistiky',
    component: StatsManagement,
    meta: {
        showInMenu: true,
        acceptedUserRoles: [0, 1]
    }
  },
  {
    path: '/rezervace',
    name: 'Nová rezervace',
    component: Rooms,
    meta: {
        showInMenu: true,
        acceptedUserRoles: [0, 1, 2, 3, 4]
    }
  },
  {
    path: '/dataily-rezervace',
    name: 'Detaily rezervace',
    component: ReservationDetails,
    meta: {
        showInMenu: false,
        parentHighlight: '/rezervace',
        acceptedUserRoles: [0, 1, 2, 3, 4]
    }
  },
  {
    path: '/potvrzeni-rezervace',
    name: 'Potvrzení rezervace',
    component: ReservationConfirmation,
    meta: {
        showInMenu: false,
        parentHighlight: '/rezervace',
        acceptedUserRoles: [0, 1, 2, 3, 4]
    }
  },
  {
    path: '/moje-rezervace',
    name: 'Moje rezervace',
    component: MyReservations,
    meta: {
        showInMenu: true,
        acceptedUserRoles: [0]
    }
  },
  {
    path: '/zavady',
    name: 'Závady',
    component: Failure,
    meta: {
        showInMenu: true,
        acceptedUserRoles: [1, 2, 3, 4]
    }
  },
    {
        path: '/uklizeni',
        name: 'Uklízení',
        component: UncleanedRooms,
        meta: {
            showInMenu: true,
            acceptedUserRoles: [1, 2, 3, 4]
        }
    },
  {
    path: '/login',
    name: 'Přihlášení',
    component: Login,
    meta: {
        showInMenu: false,
        hideMenu: true,
        acceptedUserRoles: [0]
    }
  },
  {
    path: "/:pathMatch(.*)*",
    name: 'Stránka nenalezena',
    component: () => import('../views/notFound.vue'),
    meta: {
        showInMenu: false,
        acceptedUserRoles: [0, 1, 2, 3, 4]
    }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
    document.title = to.name;
    let routerRoleId = store.getters.getLoggedUserRole
    //pokud stranka patri k roli aktualniho uzivatele
    if (to.meta.acceptedUserRoles.includes(routerRoleId)) {
        next()
        return
    }
    //aktualni uzivatel nemuze jit na tuto stranku
    else {
        //uzivatel je prihlasen a chce jit znova na login
        if (store.getters.isAuthenticated) {
            document.title = 'Home';
            next('/');
        }
        //guest chtel jit na stranku adminu -> login
        else {
            document.title = 'Přihlášení';
            next('/login');
        }
    }
});

export default router
