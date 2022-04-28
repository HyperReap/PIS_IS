<template>
  <el-button type="primary" @click="saveFailure()">Save new failure</el-button>
  <el-dialog
      v-model="dialogVisible"
      :title="'Přidat novou závadu'"
      width="40%"
  >
    <el-form :model="form" label-width="120px">
      <el-form-item label="Pokoj">
        <el-select v-model="form.room" placeholder="Vybrat pokoj">
          <el-option v-for="room in rooms" :key="room.id" :label="'Pokoj ' + room.roomNumber" :value="room.roomNumber" />
        </el-select>
      </el-form-item>
        <el-form-item label="Popis">
          <el-input v-model="form.description" />
        </el-form-item>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="dialogVisible = false">Zavřít</el-button>
        <el-button type="primary" @click="sendFailure()"
        >Odeslat</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script setup>
import { ref } from 'vue'
import { reactive } from 'vue'

const form = reactive({
  room: '',
  description: ''
})

const dialogVisible = ref(false)
</script>

<script>
export default {
  name: 'NewFailure',
  //props: ['saveFailure'],

  data() {
    return {
      rooms: [],
    };
  },
  
  created() {
    this.loadRoomNumbers();
  },
  
  methods: {
    loadRoomNumbers() {
      //this.$root.loading = !this.$root.loading
      fetch('api/Room/GetAll')
          .then(r => r.json())
          .then(json  => {
            this.rooms = json;
          });
    },
    
    saveFailure() {
      this.dialogVisible = true;
    },
    
    sendFailure(failure) {
      let requestParams = this.$createRequestParams(failure, true);
      let requestBody = this.$createBodyParams(failure);
      //this.$root.loading = !this.$root.loading
      fetch('api/Failure/Save' + requestParams, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestBody)
      })
    }
  }
}
</script>

<style scoped>
.failure-wrap {
  height: 70px;
  margin: 2%;
  flex-basis: 21%;
  flex-grow: 0;
  border: 1px solid var(--el-border-color);
}

.failure-wrap:hover{
  border: 1px solid var(--el-color-primary);
}

.failure {
  text-align: center;
}

.room-num {
  width: 25%;
  padding: 5%;
  border-right: 3px dashed var(--el-border-color);
  background-color: var(--el-fill-color-lighter);
}

.desc {
  width: 65%;
  padding: 3%;
  text-align: left;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
}

.icon {
  width: 5%;
  display: flex;
  align-items: center;
  border-left: 3px dashed var(--el-border-color);
}

.el-icon {
  margin: 3px;
}

.detail-solved {
  padding-bottom: 4px;
  border-bottom: 1px solid var(--el-border-color);
}

.detail-desc {
  padding-top: 4px;
}
</style>