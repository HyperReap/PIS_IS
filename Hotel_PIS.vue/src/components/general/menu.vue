<template>
    <el-menu router :default-active="activeLink" mode="horizontal">
        <template v-for="(item, index) in $router.options.routes">
            <el-menu-item :key="index" :index="item.path" v-if="item.meta.showInMenu == true && matchWithUser(item.meta.acceptedUserRoles)">
                {{ item.name }}
            </el-menu-item>
        </template>
        <el-menu-item v-if="showLogout" index="" @click="logout()">Odhlásit</el-menu-item>
    </el-menu>
</template>

<script lang="js">

    export default {
        data() {
            return {
                activeLink: null,
                activeUser: 0,
                showLogout: false
            };
        },
        created() {
            this.activeUser = this.$store.getters.getLoggedUserRole
            this.showLogout = this.$store.getters.isAuthenticated
        },
        methods: {
            matchWithUser(acceptedUserRoles) {
                return acceptedUserRoles.includes(this.activeUser)
            },
            logout() {
                this.$store.dispatch('logout');
                this.$router.push({ path: '/' });
            }
        },
        mounted() {
            this.activeLink = this.$route.path
        },
        watch: {
            $route(newVal, oldVal) {
                this.activeLink = !newVal.meta.parentHighlight ? newVal.path : newVal.meta.parentHighlight;
            },
            '$store.getters.getLoggedUserRole': {
                handler(newValue, oldValue) {
                    this.activeUser = newValue
                },
                immediate: true
            },
            '$store.getters.isAuthenticated': {
                handler(newValue, oldValue) {
                    this.showLogout = newValue
                },
                immediate: true
            }
        }
    };
</script>

