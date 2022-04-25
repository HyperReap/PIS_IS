<template>
    <el-menu router :default-active="activeLink" mode="horizontal">
        <template v-for="(item, index) in $router.options.routes">
            <el-menu-item :key="index" :index="item.path" v-if="item.meta.showInMenu == true">
                {{ item.name }}
            </el-menu-item>
        </template>
    </el-menu>
</template>

<script lang="js">

    export default {
        data() {
            return {
                activeLink: null,
            };
        },
        mounted() {
            this.activeLink = this.$route.path
        },
        watch: {
            $route(newVal, oldVal) {
                this.activeLink = !newVal.meta.parentHighlight ? newVal.path : newVal.meta.parentHighlight;
            }
        }
    };
</script>

