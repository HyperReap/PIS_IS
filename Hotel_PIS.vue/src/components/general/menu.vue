<template>
    <el-menu router :default-active="activeLink" mode="horizontal" class="main-menu">
        <template v-for="(item, index) in $router.options.routes">
            <el-menu-item :key="index" :index="item.path" v-if="item.meta.showInMenu == true && matchWithUser(item.meta.acceptedUserRoles)">
                {{ item.name }}
            </el-menu-item>
        </template>
        <el-menu-item index="" @click="" id="logged-user" :class="{'logged-user-info' : !isMobile}" v-if="showLogout">
            <el-dropdown>
                <span class="el-dropdown-link">
                    <el-avatar class="user-avatar"> {{initials}} </el-avatar>
                </span>
                <template #dropdown>
                    <el-dropdown-menu>
                        <div class="user-menu">
                            <div class="left">
                                <el-avatar class="user-avatar"> {{initials}} </el-avatar>
                            </div>
                            <div class="right">
                                <p class="name">{{activeUserData.firstName}} {{activeUserData.secondName}}</p>
                                <p>{{activeUserData.email}}</p>
                                <a href="#" @click="logout()" class="logout-link">Odhlásit</a>
                            </div>
                        </div>
                    </el-dropdown-menu>
                </template>
            </el-dropdown>
        </el-menu-item>
    </el-menu>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        data() {
            return {
                activeLink: null,
                activeUser: 0,
                activeUserData: {},
                showLogout: false,
                openUserInfo: false,
                isMobile: false
            };
        },
        components: {
            ElMessage
        },
        created() {
            this.activeUser = this.$store.getters.getLoggedUserRole
            this.showLogout = this.$store.getters.isAuthenticated
            this.activeUserData = this.$store.getters.getLoggedUser
            this.editMenuForMobile
            window.addEventListener("resize", this.editMenuForMobile);
        },
        computed: {
            initials() {
                try {
                    return this.activeUserData.firstName[0] + this.activeUserData.secondName[0]
                }
                catch (error) {
                    return "OS"
                }
            }
        },
        methods: {
            matchWithUser(acceptedUserRoles) {
                return acceptedUserRoles.includes(this.activeUser)
            },
            logout() {
                ElMessage({ "message": "Uživatel odhlášen", "type": "success", "custom-class": "message-class" });
                this.$store.dispatch('logout');
                this.$router.push({ path: '/' });
            },
            editMenuForMobile() {
                let target = document.getElementById("logged-user");
                if (document.getElementsByClassName("el-sub-menu__hide-arrow").length) {
                    this.isMobile = true;
                    if (target !== null) {
                        target.classList.remove("logged-user-info");
                    }
                }
                else {
                    this.isMobile = false;
                    if (target !== null) {
                        target.classList.add("logged-user-info");
                    }
                }
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
            },
            '$store.getters.getLoggedUser': {
                handler(newValue, oldValue) {
                    this.activeUserData = newValue
                },
                immediate: true
            }
        }
    };
</script>
<style>
    .main-menu{
        position: relative;
    }
    .logged-user-info {
        position: absolute !important;
        right: 0;
    }
    .logged-user-info:hover{
        background-color: #fff!important;
    }
    .user-avatar {
        background: var(--el-color-primary)!important;
        text-transform: uppercase;
        font-size: var(--el-avatar-icon-size)!important;
        line-height: 45px;
    }
    .user-menu{
        width: 280px;
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        padding: 10px;
    }
    .logout-link {
        color: var(--el-color-primary);
        text-decoration: none;
        margin-top: 10px;
        display: block;
    }
    
    .logout-link:hover{
        text-decoration: underline;
    }
    .left{
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
    .right{
        padding-left: 20px;
    }
    .name{
        font-weight: 500;
    }
</style>
