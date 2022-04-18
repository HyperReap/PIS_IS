import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import ElementPlus from 'element-plus'
import cs from 'element-plus/es/locale/lang/cs'
import * as Icons from '@element-plus/icons-vue'
import 'element-plus/dist/index.css'

//createApp(App).use(store).use(router).use(ElementPlus).mount('#app')

const app = createApp(App);
app.use(store).use(router).use(ElementPlus, {locale: cs});
for (let i in Icons) {
    app.component(i, Icons[i])
}
app.mount("#app");