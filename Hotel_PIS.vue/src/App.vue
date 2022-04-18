<template>
    <loading v-if="loading" />
    <mainMenu />
    <div class="common-layout">
        <el-container>
            <el-main>
                <router-view />
            </el-main>
        </el-container>
    </div>
</template>

<script>
    // @ is an alias to /src
    import mainMenu from '@/components/general/menu.vue'
    import loading from '@/components/general/loading.vue'

    export default {
        name: 'App',
        components: {
            mainMenu,
            loading
        },
        data() {
            return {
                loading: false,
            };
        },
    }
</script>
<style>
    @import url('https://fonts.googleapis.com/css2?family=Roboto:wght@100;300&display=swap&display=swap&subset=latin-ext');
    *{
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }
    sup{
        line-height: 0;
        margin-bottom: 10px;
    }
    #app {
      font-family: 'Roboto', Helvetica, Arial, sans-serif;
      -webkit-font-smoothing: antialiased;
      -moz-osx-font-smoothing: grayscale;
      min-height: 100vh;
    }
    .loading {
        position: absolute;
        z-index: 100000;
        width: 100%;
        height: 100%;
        background: var(--el-color-primary-light-9);
    }
</style>
