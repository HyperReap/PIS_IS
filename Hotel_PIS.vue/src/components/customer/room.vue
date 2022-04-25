<template>
    <h3>Pokoj {{ room.roomNumber }}</h3>
    <p><span>&#128719;</span> {{ room.numberOfBeds }} {{ bedsWord }}</p>
    <p><span>&#128719; <plus class="icon" /></span>{{ room.numberOfSideBeds }} {{ sideBedsWord }}</p>
    <p><full-screen class="icon" /> {{ room.roomSize }}m<sup>2</sup></p>
    <p><location class="icon" /> {{ room.floor }} patro</p>
    <p><money class="icon" />{{ room.costPerNight }} Kč/noc</p>
    <div class="equipments">
        <p v-for="equipment in equipments">
            <check class="icon-equipment" />
            {{equipment}}
        </p>
    </div>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        name: 'Room',
        props: ['room'],
        components: {
            ElMessage
        },
        data() {
            return {
                equipments: []
            };
        },
        computed: {
            bedsWord() {
                let word;
                let beds = this.room.numberOfBeds;
                switch (true) {
                    case (beds == 0 || beds > 4):
                        word = "postelí"
                        break;
                    case (beds == 1):
                        word = "postel"
                        break;
                    case (beds >= 2 && beds <= 4):
                        word = "postele"
                        break;
                    default:
                        word = "postele"
                }
                return word
            },
            sideBedsWord() {
                let word;
                let beds = this.room.numberOfBeds;
                switch (true) {
                    case (beds == 0 || beds > 4):
                        word = "přistýlek"
                        break;
                    case (beds == 1):
                        word = "přistýlka"
                        break;
                    case (beds >= 2 && beds <= 4):
                        word = "přistýlky"
                        break;
                    default:
                        word = "přistýlky"
                }
                return word
            }
        },
        created() {
            this.loadRoomEquipment();
        },
        methods: {
            loadRoomEquipment() {
                if (this.room.id) {
                    fetch('api/Room/GetEquipmentsOfRoom?roomId='+this.room.id)
                    .then(r => r.json())
                    .then(json => {
                        let self = this;
                        Object.keys(json).forEach(function (key) {
                            self.equipments.push(json[key].name);
                        });
                        return;
                    })
                    .catch(error => {
                        ElMessage.error({ "message": "Nepodařilo se načíst vybavení do pokoje!", "custom-class": "message-class", "grouping": true});
                        console.log(error);
                    });
                }
            }
        }
    };
</script>

<style scoped>
    .icon {
        color: var(--el-text-color-primary);
        width: 20px;
        margin-right: 15px
    }
    .icon-equipment {
        color: var(--el-color-primary);
        width: 20px;
        margin-right: 4px;
    }
    h3 {
        flex-basis: 100%;
        margin-bottom: 20px;
        color: var(--el-text-color-primary);
    }
    p {
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        align-items: center;
        justify-content: flex-start;
        flex-basis: calc(100% / 3);
        margin-bottom: 5px;
        color: var(--el-text-color-primary);
    }

    p span{
        line-height: 0;
        font-size: 26px;
        display: block;
        margin-right: 9px;
        padding-bottom: 5px;
        position: relative;
    }
    p span .icon{
        position: absolute;
        width: 10px;
        right: 0px;
        top: -12px;
    }
    .equipments {
        flex-basis: 100%;
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: flex-start;
    }
</style>