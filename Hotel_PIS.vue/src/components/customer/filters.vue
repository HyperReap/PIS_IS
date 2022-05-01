<template>
    <div :class="[{ 'filters-closed': !openFilters }, 'filters']">
        <el-button class="filters-opener" @click="openFilters = !openFilters">{{filtersOpened}}</el-button>
        <h4>Filtry</h4>
        <el-date-picker v-model="filteredValues.dateFrom" type="date" placeholder="Datum příjezdu" popper-class="date-dropdown"
                        class="date-picker" :disabled-date="disabledDateFrom" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
        <el-date-picker v-model="filteredValues.dateTo" type="date" placeholder="Datum odjezdu" popper-class="date-dropdown"
                        class="date-picker" :disabled-date="disabledDateTo" format="DD. MM. YYYY" value-format="YYYY-MM-DD" />
        <div class="order-4">
            <p>Cena</p>
            <el-slider @change="setFilterPrice" range :min="filterValues.minPrice" :max="filterValues.maxPrice" class="range-picker" tooltip-class="range-picker-tooltip" />
        </div>
        <div class="order-5">
            <p>Počet postelí</p>
            <el-slider @change="setFilterBeds" range :min="filterValues.minNumberOfBeds" :max="filterValues.maxNumberOfBeds" class="range-picker" tooltip-class="range-picker-tooltip" />
        </div>
        <div class="order-3">
            <el-select v-model="filteredValues.equipments" multiple collapse-tags placeholder="Vybavení">
                <el-option v-for="item in equipments" :key="item.value" :label="item.label" :value="item.label" />
            </el-select>
        </div>
        <el-button class="button" @click="filterRooms">Filtrovat</el-button>
    </div>
</template>
<script lang="js">
    import { ElMessage } from 'element-plus'
    export default {
        props: ['filterValues'],
        components: {
            ElMessage
        },
        data() {
            return {
                filteredValues: {
                    equipments: [],
                    dateFrom: null,
                    dateTo: null,
                    minPrice: null,
                    maxPrice: null,
                    minNumberOfBeds: null,
                    maxNumberOfBeds: null                    
                },
                equipments: [],
                dayDuration: 86400000,
                openFilters: false
            };
        },
        created() {
            this.loadAllEquipment();
        },
        computed: {
            filtersOpened() {
                return this.openFilters ? "Skrýt filtry" : "Zobrait filtry"
            }
        },
        methods: {
            disabledDateFrom(date) {
                return date.getTime() < Date.now() - this.dayDuration
            },
            disabledDateTo(date) {
                if (this.filteredValues.dateFrom === null) {
                    return date.getTime() < Date.now()
                }
                else {
                    return date.getTime() < this.getTimeStamp(this.filteredValues.dateFrom)
                }
            },
            getTimeStamp(strDate) {
                return ((new Date(strDate).getTime()))
            },
            setFilterPrice(prices) {
                this.filteredValues.minPrice = prices[0]
                this.filteredValues.maxPrice = prices[1]
            },
            setFilterBeds(beds) {
                this.filteredValues.minNumberOfBeds = beds[0]
                this.filteredValues.maxNumberOfBeds = beds[1]
            },
            filterRooms() {
                this.$emit('filterRooms', this.filteredValues)
            },
            loadAllEquipment() {
                fetch('api/Room/GetEquipments')
                .then(r => r.json())
                .then(json => {
                    let self = this;
                    Object.keys(json).forEach(function (key) {
                        self.equipments.push({ "label": json[key].name, "value": json[key].id });
                    });
                    return;
                })
                .catch(error => {
                    ElMessage.error({ "message": "Nepodařilo se vybavení do filtrů!", "custom-class": "message-class"});
                    console.log(error);
                });
            }
        }
    }
</script>
<style scoped>
    .filters {
        background: var(--el-color-primary);
        color: white;
        position: fixed;
        width: 100%;
        padding: 20px;
        max-width: calc(16.6666666667% - 30px);
        display: flex;
        flex-direction: column;
    }
    h4 {
        margin-bottom: 20px;
        text-transform: uppercase;
    }
    .filters >>> .date-picker{
        width: 100%!important;
        margin-bottom: 20px;
    }
    .filters >>> .range-picker .el-slider__bar{
        background: #fff!important;
    }
    .filters >>> .range-picker .el-slider__runway{
        background: #a9abb0!important;
    }
    .filters >>> .range-picker .el-slider__button{
        border-color: #344055;
    }    
    .filters >>> .el-button.button{
        margin: 20px 0 0 auto;
        order: 6;
    }
    .filters >>> .el-button.button:hover,
    .filters >>> .el-button.button:focus,
    .filters >>> .filters-opener:hover,
    .filters >>> .filters-opener:focus
    {
        color: var(--el-button-text-color);
        border-color: var(--el-button-border-color);
        background-color: var(--el-button-bg-color);
    }
    .order-3 >>> .el-select{
        width: 100%;
    }
    .filters-opener{
        display: none;
    }
    .filters >>> .date-picker .el-input__inner{
            padding-left: 30px!important;
    }
    @media screen and (max-width: 1200px) {
        .filters {
            position: relative;
            max-width: unset;
            margin-bottom: 20px;
            display: flex;
            flex-direction: row;
            flex-wrap: wrap;
            justify-content: space-between;
            align-items: flex-start;
        }
        .filters h4{
            flex-basis: 100%
        }
        .filters >>> .date-picker{
            width: 30%!important;
            flex-basis: 30%;
        }
        .order-3{
            flex-basis: 30%;
            order: 3;
        }
        .order-4,
        .order-5{
            flex-basis: 45%;
        }
        .order-4{
            order: 4;
        }
        .order-5{
            order: 5;
        }
        .filters >>> .el-button.button {
            order: 6;
            flex-basis: 100%;
            margin: 20px 0 0 calc(100% - 80px);
        }
    }
    @media screen and (max-width: 680px) {
        .filters:not(.filters-closed) .filters-opener {
            display: block;
            position: absolute;
            left: 20px;
            bottom: 20px;
        }
        .filters-closed .filters-opener
        {
            display: block;
            position: absolute;
            right: 20px;
            top: 20px;
        }        
        .filters-closed >>> .date-picker,
        .filters-closed .order-3,
        .filters-closed .order-4,
        .filters-closed .order-5,
        .filters-closed >>> .button
        {
            display: none !important
        }
        .filters{
            flex-direction: column;
        }
        .filters >>> .date-picker{
            width: 100%!important;
            flex-basis: 100%;
        }
        .order-3{
            flex-basis: 100%;
            width: 100%;
            order: 3;
        }
        .order-4,
        .order-5{
            flex-basis: 100%;
            width: 100%;
        }
    }
</style>
<style>
    .date-dropdown *,
    .range-picker-tooltip,
    .el-select-dropdown__item {
        font-family: 'Roboto', Helvetica, Arial, sans-serif;
    }
</style>