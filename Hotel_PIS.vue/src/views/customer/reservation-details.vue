<template>
    <el-row>
        <el-col :span="24">
            <h2>Detaily rezervace</h2>
            <div v-if="rooms.length">
                <ul>
                    <li>zobrazit zde detaily ohledne vybranych pokoju, tak jako na strance se vsemi pokoji</li>
                    <li>donutit uživatele vybrat správně datum</li>
                    <li>při vybírání data už musí být vypnuté obsazené termíny</li>
                    <li>donutit ho vyplnit všechy potřebné údaje (jmeno, prijmeni, email, telefon, pocet osob - nesmi byt vyssi nez max pocet osob na pokoji/pokojich)</li>
                    <li>odesilani na BE 2x (prvne klient, melo by se vratit jeho id ; pak rezervace, mel bych poslat id klienta)</li>
                    <li>mel bych si do store ulozit email klienta pro pouziti v mych rezervacich</li>
                    <li>mel bych presmerovat na stranku s detaily rezervace</li>
                </ul>
                {{rooms}}
            </div>
            <div v-else>
                <h3>Nejsou vybrány žádné pokoje k rezervaci!</h3>
                <p class="countdown">Přesměrování na stránku pro výběr pokojů proběhne za {{countDown}} {{secWord}}</p>
            </div>
        </el-col>
    </el-row>
</template>
<script lang="js">
    export default {
        components: {
        },
        data() {
            return {
                rooms: [1, 2],
                countDownEnabled: false,
                countDown: 10,
            };
        },
        created() {
            //this.rooms = this.$store.getters.getReservationRooms.rooms;
            this.countDownEnabled = this.rooms.length ? false : true;
            this.loadRooms();
        },
        computed: {
            secWord() {
                let word;
                let secs = this.countDown;
                switch (true) {
                    case (secs == 0 || secs > 4):
                        word = "sekund"
                        break;
                    case (secs == 1):
                        word = "sekundu"
                        break;
                    case (secs >= 2 && secs <= 4):
                        word = "sekundy"
                        break;
                    default:
                        word = "sekund"
                }
                return word
            },
        },
        methods: {
            loadRooms() {
                this.$root.loading = !this.$root.loading;
                this.rooms.forEach(id => {
                    fetch('api/Room/Get?id=' + id, {
                        method: 'GET',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    })
                    .then(r => r.json())
                    .then(json => {
                        console.log(json);
                        //TODO: fix ; musi se provest po dokonceni foreach
                        this.$root.loading = !this.$root.loading
                        return
                    })
                });            
            },
        },
        watch: {
            countDownEnabled(value) {
                if (value) {
                    setTimeout(() => {
                        this.countDown--;
                    }, 1000);
                }
            },
            countDown: {
                handler(value) {
                    if (value > 0 && this.countDownEnabled) {
                        setTimeout(() => {
                            this.countDown--;
                        }, 1000);
                    }
                    if(value == 0 && this.countDownEnabled){
                        this.$router.push({ path: '/rezervace' });
                    }
                },
                immediate: true
            }
        }
    };
</script>
<style scoped>
    h2{
        margin-bottom: 20px;
        text-align: center;
    }
    h3,
    .countdown {
        text-align: center;
        margin-bottom: 10px;
    }
</style>