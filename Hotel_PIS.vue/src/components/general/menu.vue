<template>
    <el-menu router :default-active="$route.path" mode="horizontal">
        <template v-for="(rule, index) in $router.options.routes">
            <el-menu-item :key="index" :index="rule.path">
                {{ rule.name }}
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
                this.activeLink = newVal.path
            }
        }
    };
</script>

