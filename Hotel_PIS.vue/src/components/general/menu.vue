<template>
    <el-menu :default-active="defaultActive" mode="horizontal">
            <el-menu-item index="1" route="/">
                Home
            </el-menu-item>
        <router-link to="/pokoje">
            <el-menu-item index="2">
                Pokoje
            </el-menu-item>
        </router-link>
        <router-link to="/moje-rezervace">
            <el-menu-item index="3">
                Moje rezervace
            </el-menu-item>
        </router-link>
    </el-menu>
</template>
<script>
    export default {
        data() {
            return {
                defaultActive: '1'
            }
        }
    }
</script>